package environment.map;

import system.objects.ResourceObject;
import utils.FrogException;

public class GameMapLayer implements ResourceObject
{
	private Integer[][] tiles;
	private int width, height;
	
	public GameMapLayer(int width, int height)
	{
		this.tiles = new Integer[height][width];
		this.width = width;
		this.height = height;
	}
	
	public void load()
	{
		/*for(int x = 0; x < this.width; x++)
		{
			for(int y = 0; y < this.height; y++)
			{
				// Load Tile (x, y)
			}
		}*/
	}
	
	public void unload()
	{
		
	}
	
	/**
	 * Obtient la valeur de la tuile à la position donnée
	 * @param relativeX La valeur horizontale relative au chunk de la position
	 * @param relativeY La valeur verticale relative au chunk de la position
	 * @return La valeur de la tuile à la position donnée, null si inexistant
	 */
	public Integer getTile(int relativeX, int relativeY)
	{
		if(relativeX >= 0 && relativeX < this.width && relativeY >= 0 && relativeY < this.height)
			return this.tiles[relativeY][relativeX];
		return null;
	}
	
	/**
	 * Met à jour la valeur de la tuile à la position donnée
	 * @param relativeX La valeur horizontale relative au chunk de la position donnée
	 * @param relativeY La valeur verticale relative au chunk de la position donnée
	 * @param value La nouvelle valeur de la tuile
	 * @throws FrogException Exception jetée lorsque la position donnée est située en dehors des limites définies
	 */
	public void setTile(int relativeX, int relativeY, Integer value) throws FrogException
	{
		if(relativeX >= 0 && relativeX < this.width && relativeY >= 0 && relativeY < this.height)
			this.tiles[relativeY][relativeX] = value;
		else
			throw new FrogException("Tentative de modification d'une tuile en dehors des limites définies");
	}
}
