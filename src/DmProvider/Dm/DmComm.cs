namespace Dm
{
	internal class DmComm
	{
		internal virtual void Send(DmMsg msgIn, int SendTimeOut, bool crcBody, bool encryptMsg)
		{
		}

		internal virtual DmMsg Recv(DmMsg RecvMsg, int RecvTimeOut, bool crcBody, bool encryptMsg)
		{
			return null;
		}
	}
}
