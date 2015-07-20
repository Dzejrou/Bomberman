﻿using System;
using System.IO;
using System.Drawing;
using System.Net.Sockets;

namespace Bomberman
{
	class Connection
	{
		public TcpClient client = null;
		public StreamWriter writer;
		public StreamReader reader;
		public Point position;
		public int playerNumber;

		public Connection(TcpClient client)
		{
			this.client = client;
			this.writer = new StreamWriter(client.GetStream());
			this.reader = new StreamReader(client.GetStream());
			this.writer.AutoFlush = true;
		}
	}
}
