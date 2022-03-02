using System;
using System.Collections;

namespace Dm
{
	internal class DmCommandSet
	{
		private static readonly string ClassName = "DmCommandSet";

		private BatchedCommand m_NowBatchCmd;

		private ArrayList m_BatchCommands;

		private DmConnection m_Conn;

		public ArrayList BatchCommands => m_BatchCommands;

		public DmCommandSet()
		{
			m_BatchCommands = new ArrayList();
		}

		public void Append(DmCommand cmd)
		{
			if (m_Conn == null)
			{
				m_Conn = cmd.do_DbConnection;
			}
			if (m_NowBatchCmd == null)
			{
				BatchedCommand batchedCommand = new BatchedCommand(cmd);
				batchedCommand.AddParameters(cmd);
				m_NowBatchCmd = batchedCommand;
				m_BatchCommands.Add(batchedCommand);
			}
			else if (!m_NowBatchCmd.Text.Equals(cmd.do_CommandText))
			{
				BatchedCommand batchedCommand2 = new BatchedCommand(cmd);
				batchedCommand2.AddParameters(cmd);
				m_NowBatchCmd = batchedCommand2;
				m_BatchCommands.Add(batchedCommand2);
			}
			else
			{
				m_NowBatchCmd.AddParameters(cmd);
			}
		}

		public int ExecuteNonQuery()
		{
			int result = 0;
			DmTrace.TraceMethodEnter(TraceLevel.Debug, ClassName, "ExecuteNonQuery()");
			if (m_Conn == null)
			{
				throw new InvalidOperationException();
			}
			foreach (BatchedCommand batchCommand in m_BatchCommands)
			{
				DmCommand dmCommand = new DmCommand();
				dmCommand.Connection = m_Conn;
				dmCommand.do_CommandText = batchCommand.Text;
				foreach (DmParameter parameter in batchCommand.Parameters)
				{
					dmCommand.do_DbParameterCollection.do_Add(parameter);
				}
			}
			m_NowBatchCmd = null;
			m_BatchCommands = new ArrayList();
			return result;
		}

		public void Clear()
		{
			m_NowBatchCmd = null;
			m_BatchCommands = new ArrayList();
		}
	}
}
