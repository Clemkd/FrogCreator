package concurrent;

import java.util.concurrent.BlockingQueue;

import net.request.NetRequest;
import net.request.NetRequestResult;
import program.Program;

public class RequestExecutor extends Thread
{
	private BlockingQueue<FrogTask> queue;
	
	public RequestExecutor(BlockingQueue<FrogTask> queue)
	{
		this.queue = queue;
	}
	
	@Override
	public void run()
	{
		while(!Program.isStopped())
		{
			try 
			{
				// Prend une tâche à réaliser
				FrogTask task = this.queue.take();
				// Extrait la requête associée
				NetRequest request = task.getRequest();
				NetRequestResult result = new NetRequestResult(); // Resultat de l'execution
				
				// Recherche et execution synchronisée de la requete
				switch(request.getType())
				{
					case CONNECT:
						System.out.println("Connect request synch execution DONE");
						break;
					default:
						System.out.println("Default request received...");
						break;
				}
				
				// Extrait le callback de la requête
				RequestExecutionFinishedListener callback = task.getCallback();
				if(callback != null)
					callback.requestExecutionFinished(result);
			} 
			catch (InterruptedException e) 
			{
				e.printStackTrace();
			}
		}
	}
}
