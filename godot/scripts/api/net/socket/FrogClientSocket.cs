using System.Net.Sockets;
using System.Text.Json;
using FrogCreator.Api.Utils;

namespace FrogCreator.Api.Net.Socket;

/// <summary>
/// Gestionnaire de packets internet de jeu.
/// Classe permettant de gérer la communication avec le serveur de jeu.
/// </summary>
public class FrogClientSocket : IPacketListener
{
    public static readonly string PROTOCOL_VERSION = "v0.0.0.1";

    private static readonly int DURATION_BETWEEN_TENTATIVES = 1000;
    private static readonly int TENTATIVES = 10;

    private readonly List<PacketSubscriber> _balancers;
    private TcpClient? _socket;
    private string? _token;
    private bool _isRunning;
    private StreamReader? _reader;
    private StreamWriter? _writer;
    private readonly object _lock = new object();

    public FrogClientSocket()
    {
        _balancers = new List<PacketSubscriber>();
        _isRunning = false;
    }

    /// <summary>
    /// Ajoute un nouveau aiguilleur de packets.
    /// Les souscripteurs de l'aiguilleur recevront les packets en reception.
    /// </summary>
    /// <param name="balancer">Le nouveau aiguilleur à ajouter</param>
    public void AddPacketSubscribers(PacketSubscriber balancer)
    {
        lock (_lock)
        {
            _balancers.Add(balancer);
        }
    }

    /// <summary>
    /// Supprime un aiguilleur de packets.
    /// Les souscripteurs de l'aiguilleur ne recevront plus les packets en reception.
    /// </summary>
    /// <param name="balancer">L'aiguilleur à supprimer</param>
    public void RemovePacketSubscribers(PacketSubscriber balancer)
    {
        lock (_lock)
        {
            _balancers.Remove(balancer);
        }
    }

    /// <summary>
    /// Démarre la communication avec le serveur de jeu si possible
    /// </summary>
    /// <param name="ip">L'adresse du serveur hôte</param>
    /// <param name="port">Le port d'écoute du serveur hôte</param>
    public void Start(string ip, int port)
    {
        if (IsRunning())
            return;

        if (TrySocketConnect(ip, port, TENTATIVES, DURATION_BETWEEN_TENTATIVES))
        {
            _isRunning = true;
            PacketReaderWorker worker = new PacketReaderWorker(this);
            worker.AddListener(this);
            worker.Start(); // Lancement du thread de lecture des packets en reception
        }
    }

    public void OnPacketReceived(Packet packet)
    {
        switch (packet.GetPacketType())
        {
            case PacketType.CONNECT_RESULT:
                if (_token == null)
                {
                    using JsonDocument doc = JsonDocument.Parse(packet.GetSerializedObject());
                    JsonElement root = doc.RootElement;
                    bool result = root.GetProperty("result").GetBoolean();
                    if (result)
                        _token = root.GetProperty("token").GetString();
                }
                break;
            default:
                break;
        }

        RaiseEventToBalancers(packet);
    }

    /// <summary>
    /// Obtient l'état de la communication avec le serveur de jeu
    /// </summary>
    /// <returns>True si la communication est active, False dans le cas contraire</returns>
    public bool IsRunning()
    {
        lock (_lock)
        {
            return _isRunning;
        }
    }

    /// <summary>
    /// Défini l'arrêt de la communication avec le serveur de jeu
    /// </summary>
    public void Stop()
    {
        lock (_lock)
        {
            _isRunning = false;
        }
    }

    /// <summary>
    /// Obtient le gestionnaire de flux en entrée
    /// </summary>
    public StreamReader? GetReader()
    {
        return _reader;
    }

    /// <summary>
    /// Obtient le gestionnaire de flux en sortie
    /// </summary>
    public StreamWriter? GetWriter()
    {
        return _writer;
    }

    /// <summary>
    /// Tentative de connection avec l'hôte distant.
    /// Tente une connexion avec l'hôte distant spécifié.
    /// Si la connexion échoue, une nouvelle tentative est relancé au bout d'un temps défini
    /// et celà un nombre de fois défini également.
    /// </summary>
    /// <param name="ip">L'IP du serveur hôte</param>
    /// <param name="port">Le PORT d'écoute du serveur hôte</param>
    /// <param name="tentativesCount">Le nombre de tentatives de connexion maximum</param>
    /// <param name="millisecondsBetweenEachTentatives">Le temps d'attente entre chaque tentative en milisecondes</param>
    /// <returns>True si la connexion a été réalisée avec succès, False dans le cas contraire</returns>
    /// <exception cref="FrogException">Jetée lorsqu'une erreur système survient</exception>
    private bool TrySocketConnect(string ip, int port, int tentativesCount, int millisecondsBetweenEachTentatives)
    {
        bool result = false;
        int tentatives = 0;

        try
        {
            while (!result && tentatives <= tentativesCount)
            {
                try
                {
                    _socket = new TcpClient(ip, port);
                    result = true;
                }
                catch (Exception e)
                {
                    System.Console.Error.WriteLine(e.Message);
                    result = false;
                    tentatives++;
                    Thread.Sleep(millisecondsBetweenEachTentatives);
                }
            }

            if (result && _socket != null)
            {
                NetworkStream stream = _socket.GetStream();
                _reader = new StreamReader(stream);
                _writer = new StreamWriter(stream) { AutoFlush = true };

                var versionObj = new Dictionary<string, object> { { "version", PROTOCOL_VERSION } };
                SendPacket(new Packet(PacketType.PROTOCOL_VERSION, JsonSerializer.Serialize(versionObj)));
                string? responseLine = _reader.ReadLine();
                if (responseLine == null)
                    throw new FrogException("Pas de réponse du serveur");

                Packet p = Packet.GetPacket(responseLine);

                if (p.GetPacketType() != PacketType.PROTOCOL_VERSION_RESULT)
                    throw new FrogException("Mauvais packet reçu; Attendu : PROTOCOL_VERSION_RESULT");

                using JsonDocument versionDoc = JsonDocument.Parse(p.GetSerializedObject());
                bool versionResult = versionDoc.RootElement.GetProperty("result").GetBoolean();

                if (!versionResult)
                {
                    _isRunning = false;
                    throw new FrogException("Mauvaise version du protocol de communication");
                }
            }
        }
        catch (FrogException)
        {
            throw;
        }
        catch (Exception e)
        {
            throw new FrogException(e.Message);
        }

        return result;
    }

    /// <summary>
    /// Tentative de connexion au serveur de jeu.
    /// Tente une connexion au serveur de jeu avec les informations d'authentification spécifiées.
    /// </summary>
    /// <param name="account">Le nom de compte joueur</param>
    /// <param name="password">Le mot de passe du compte joueur</param>
    public void Connect(string account, string password)
    {
        SendPacket(new Packet(PacketType.CONNECT, "Nothing"));
    }

    public void SendPacket(Packet packet)
    {
        if (_socket == null || !_socket.Connected)
            throw new FrogException("Tentative de connection au serveur de jeu sur un channel de communication non établi");

        _writer?.WriteLine(packet.ToJSON());
    }

    /// <summary>
    /// Obtient l'état de la communication avec l'hôte
    /// </summary>
    /// <returns>True si la communication est active, False dans le cas contraire</returns>
    public bool IsConnected()
    {
        lock (_lock)
        {
            if (_socket != null)
                return _socket.Connected;
            return false;
        }
    }

    /// <summary>
    /// Obtient le token de communication avec le serveur de jeu résultant de l'authentification
    /// </summary>
    /// <returns>Null si l'authentification n'a pas été réalisée, ou a échouée, une clé dans le cas contraire</returns>
    public string? GetToken()
    {
        return _token;
    }

    private void RaiseEventToBalancers(Packet packet)
    {
        for (int i = 0; i < _balancers.Count; i++)
        {
            _balancers[i].PushPacket(packet);
        }
    }
}
