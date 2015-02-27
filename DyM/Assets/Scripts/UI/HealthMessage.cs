using UnityEngine;
using System.Collections;

public class HealthMessage 
{
    private int message;
    public int Message {get {return message;}}
	public HealthMessage(int message)
    {
        this.message = message;
    }
}
