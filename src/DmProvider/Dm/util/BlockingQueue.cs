using System;
using System.Collections.Generic;
using System.Threading;

namespace Dm.util
{
	internal class BlockingQueue<T>
	{
		private Queue<T> _queue;

		private int _limit = -1;

		internal int Count
		{
			get
			{
				int num = 0;
				lock (_queue)
				{
					return _queue.Count;
				}
			}
		}

		internal BlockingQueue()
			: this(-1)
		{
		}

		internal BlockingQueue(int limit)
		{
			_limit = limit;
			_queue = new Queue<T>();
		}

		internal void Enqueue(T item)
		{
			lock (_queue)
			{
				while (_limit != -1 && _queue.Count > _limit)
				{
					Monitor.Wait(_queue, 1000);
				}
				_queue.Enqueue(item);
				Monitor.PulseAll(_queue);
			}
		}

		internal List<T> Dequeue(int batchSize)
		{
			List<T> list = new List<T>();
			lock (_queue)
			{
				while (_queue.Count == 0)
				{
					Monitor.Wait(_queue, 1000);
				}
				int num = Math.Min(batchSize, _queue.Count);
				while (num-- > 0)
				{
					list.Add(_queue.Dequeue());
				}
				Monitor.PulseAll(_queue);
				return list;
			}
		}

		internal void Enqueue(T item, out bool succ)
		{
			succ = false;
			lock (_queue)
			{
				if (_limit == -1 || _queue.Count < _limit)
				{
					_queue.Enqueue(item);
					succ = true;
				}
			}
		}

		internal T Dequeue()
		{
			T result = default(T);
			lock (_queue)
			{
				if (_queue.Count > 0)
				{
					return _queue.Dequeue();
				}
				return result;
			}
		}
	}
}
