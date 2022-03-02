using System.Collections;

namespace Dm
{
	internal sealed class BatchedCommand
	{
		private string m_Text;

		private ArrayList m_Parameters;

		public string Text
		{
			get
			{
				return m_Text;
			}
			set
			{
				m_Text = value;
			}
		}

		public ArrayList Parameters => m_Parameters;

		public BatchedCommand(DmCommand cmd)
		{
			m_Text = cmd.do_CommandText;
			m_Parameters = new ArrayList();
		}

		public void AddParameters(DmCommand cmd)
		{
			foreach (DmParameter item in cmd.do_DbParameterCollection)
			{
				DmParameter value = item.Clone();
				m_Parameters.Add(value);
			}
		}
	}
}
