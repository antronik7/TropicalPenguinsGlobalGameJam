using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdHelper
{
	private Queue<int> availableIds;

	public IdHelper(int idCount)
	{
		Initialize(idCount);
	}

	public void Initialize(int idCount)
	{
		availableIds = new Queue<int>(idCount);
		for (int i = 0; i < idCount; i++)
		{
			availableIds.Enqueue(i);
		}
	}

	public int GetFreeId()
	{
		if (availableIds != null || availableIds.Count > 0)
		{
			return availableIds.Dequeue();
		}
		else
		{
			throw new System.Exception("No Ids available for grab :(");
		}
	}

	public void ReleaseId(int id)
	{
		if (availableIds.Contains(id))
		{
			throw new System.Exception("Id is already in the IdHelper queue.");
		}
		else
		{
			availableIds.Enqueue(id);
		}
	}
}
