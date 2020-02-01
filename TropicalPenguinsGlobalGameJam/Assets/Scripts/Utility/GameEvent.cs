using System;
using UnityEngine.Events;

/// <summary>
/// GamePlay Events for Player Client and Server
/// </summary>
// no parameter
public class GameEvent
{
	public event Action CustomEvent;

	public virtual void Invoke()
	{
		CustomEvent?.Invoke();
	}

	public virtual void AddListener(Action callback)
	{
		CustomEvent += callback;
	}

	public virtual void RemoveListener(Action callback)
	{
		CustomEvent -= callback;
	}
}

public class GameEventU
{
	public UnityEvent CustomEvent = new UnityEvent();

	public virtual void Invoke()
	{
		CustomEvent?.Invoke();
	}

	public virtual void AddListener(UnityAction callback)
	{
		CustomEvent.AddListener(callback);
	}

	public virtual void RemoveListener(UnityAction callback)
	{
		CustomEvent.RemoveListener(callback);
	}
}

// 1 parameter
public class GameEvent<T1>
{
	public event Action<T1> CustomEvent;

	public virtual void Invoke(T1 param1)
	{
		CustomEvent?.Invoke(param1);
	}

	public virtual void AddListener(Action<T1> callback)
	{
		CustomEvent += callback;
	}

	public virtual void RemoveListener(Action<T1> callback)
	{
		CustomEvent -= callback;
	}
}

// 1 parameter
public class GameEventU<T1>
{
	[Serializable]
	public class Event : UnityEvent<T1>
	{
	}

	public UnityEvent<T1> CustomEvent = new Event();

	public virtual void Invoke(T1 param1)
	{
		CustomEvent?.Invoke(param1);
	}

	public virtual void AddListener(UnityAction<T1> callback)
	{
		CustomEvent.AddListener(callback);
	}

	public virtual void RemoveListener(UnityAction<T1> callback)
	{
		CustomEvent.RemoveListener(callback);
	}
}


// 2 parameters
public class GameEvent<T1, T2>
{
	public event Action<T1, T2> CustomEvent;

	public virtual void Invoke(T1 param1, T2 param2)
	{
		CustomEvent?.Invoke(param1, param2);
	}

	public virtual void AddListener(Action<T1, T2> callback)
	{
		CustomEvent += callback;
	}

	public virtual void RemoveListener(Action<T1, T2> callback)
	{
		CustomEvent -= callback;
	}
}

// 2 parameters
public class GameEventU<T1, T2>
{
	[Serializable]
	public class Event : UnityEvent<T1, T2>
	{
	}

	public UnityEvent<T1, T2> CustomEvent = new Event();

	public virtual void Invoke(T1 param1, T2 param2)
	{
		CustomEvent?.Invoke(param1, param2);
	}

	public virtual void AddListener(UnityAction<T1, T2> callback)
	{
		CustomEvent.AddListener(callback);
	}

	public virtual void RemoveListener(UnityAction<T1, T2> callback)
	{
		CustomEvent.RemoveListener(callback);
	}
}

// 3 parameters
public class GameEvent<T1, T2, T3>
{
	public event Action<T1, T2, T3> CustomEvent;

	public virtual void Invoke(T1 param1, T2 param2, T3 param3)
	{
		CustomEvent?.Invoke(param1, param2, param3);
	}

	public virtual void AddListener(Action<T1, T2, T3> callback)
	{
		CustomEvent += callback;
	}

	public virtual void RemoveListener(Action<T1, T2, T3> callback)
	{
		CustomEvent -= callback;
	}
}

// 4 parameters
public class GameEvent<T1, T2, T3, T4>
{
	public event Action<T1, T2, T3, T4> CustomEvent;

	public virtual void Invoke(T1 param1, T2 param2, T3 param3, T4 param4)
	{
		CustomEvent?.Invoke(param1, param2, param3, param4);
	}

	public virtual void AddListener(Action<T1, T2, T3, T4> callback)
	{
		CustomEvent += callback;
	}

	public virtual void RemoveListener(Action<T1, T2, T3, T4> callback)
	{
		CustomEvent -= callback;
	}
}

// 5 parameters
public class GameEvent<T1, T2, T3, T4, T5>
{
	public event Action<T1, T2, T3, T4, T5> CustomEvent;

	public virtual void Invoke(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5)
	{
		CustomEvent?.Invoke(param1, param2, param3, param4, param5);
	}

	public virtual void AddListener(Action<T1, T2, T3, T4, T5> callback)
	{
		CustomEvent += callback;
	}

	public virtual void RemoveListener(Action<T1, T2, T3, T4, T5> callback)
	{
		CustomEvent -= callback;
	}
}

// 6 parameters
public class GameEvent<T1, T2, T3, T4, T5, T6>
{
	public event Action<T1, T2, T3, T4, T5, T6> CustomEvent;

	public virtual void Invoke(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6)
	{
		CustomEvent?.Invoke(param1, param2, param3, param4, param5, param6);
	}

	public virtual void AddListener(Action<T1, T2, T3, T4, T5, T6> callback)
	{
		CustomEvent += callback;
	}

	public virtual void RemoveListener(Action<T1, T2, T3, T4, T5, T6> callback)
	{
		CustomEvent -= callback;
	}
}

// 7 parameters
public class GameEvent<T1, T2, T3, T4, T5, T6, T7>
{
	public event Action<T1, T2, T3, T4, T5, T6, T7> CustomEvent;

	public virtual void Invoke(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7)
	{
		CustomEvent?.Invoke(param1, param2, param3, param4, param5, param6, param7);
	}

	public virtual void AddListener(Action<T1, T2, T3, T4, T5, T6, T7> callback)
	{
		CustomEvent += callback;
	}

	public virtual void RemoveListener(Action<T1, T2, T3, T4, T5, T6, T7> callback)
	{
		CustomEvent -= callback;
	}
}

// 8 parameters
public class GameEvent<T1, T2, T3, T4, T5, T6, T7, T8>
{
	public event Action<T1, T2, T3, T4, T5, T6, T7, T8> CustomEvent;

	public virtual void Invoke(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8)
	{
		CustomEvent?.Invoke(param1, param2, param3, param4, param5, param6, param7, param8);
	}

	public virtual void AddListener(Action<T1, T2, T3, T4, T5, T6, T7, T8> callback)
	{
		CustomEvent += callback;
		System.Runtime.CompilerServices.RuntimeHelpers.PrepareMethod(callback.Method.MethodHandle);
		System.Runtime.CompilerServices.RuntimeHelpers.PrepareDelegate(callback);
		System.Runtime.CompilerServices.RuntimeHelpers.PrepareDelegate(CustomEvent);
	}

	public virtual void RemoveListener(Action<T1, T2, T3, T4, T5, T6, T7, T8> callback)
	{
		CustomEvent -= callback;
	}
}
