using System;
using System.Xml;
using Server;
using Server.Mobiles;
using Server.Accounting;

namespace Server.Jailing
{
 	public class JailInfo
	{
		private string m_Comentarios;
		private Account m_Convicto;
		private Mobile m_Jailer;
		private DateTime m_DateCreation;
		private TimeSpan m_Duration;

		public static Point3D DefLocation = new Point3D(5301,1184,0);

		public string Comentarios{ get{ return m_Comentarios; } set{ m_Comentarios = value; } }
		public TimeSpan Duration{ get{ return m_Duration; } set{ m_Duration = value; } }
		public Account Convicto{ get{ return m_Convicto; } }
		public Mobile Jailer{ get{ return m_Jailer; } }
		public DateTime DateCreation{ get{ return m_DateCreation; } }

		public bool Active{ get{ return ( DateTime.Now < m_DateCreation + m_Duration ); } }

		public JailInfo( Account convicto, Mobile jailer, string comentarios )
		{
			m_Convicto = convicto;
			m_Jailer = jailer;
			m_Comentarios = comentarios;
                        m_DateCreation = DateTime.Now;
                        m_Duration = TimeSpan.MaxValue;
		}

                public JailInfo( XmlElement node )
		{
			m_Convicto = Accounts.GetAccount( Accounts.GetText( node["convict"], "empty" ) );

			try
			{
				int serial = Accounts.GetInt32( Accounts.GetText( node["jailer"], "0" ), 0 );
                		m_Jailer = World.FindMobile( serial );
			}
			catch
			{
			}

			m_DateCreation = Accounts.GetDateTime( Accounts.GetText( node["created"], null ), DateTime.Now );
			m_Duration = Accounts.GetTimeSpan( Accounts.GetText( node["duration"], null ), TimeSpan.Zero );
			m_Comentarios = Accounts.GetText( node["comment"], null );
		}

		public void Save( XmlTextWriter xml )
		{
			xml.WriteStartElement( "jailing" );

			xml.WriteStartElement( "convict" );
			xml.WriteString( m_Convicto.ToString() );
			xml.WriteEndElement();

			xml.WriteStartElement( "jailer" );
			xml.WriteString( m_Jailer.Serial.Value.ToString() );
			xml.WriteEndElement();

			xml.WriteStartElement( "created" );
			xml.WriteString( XmlConvert.ToString( m_DateCreation ) );
			xml.WriteEndElement();

			xml.WriteStartElement( "duration" );
			xml.WriteString( XmlConvert.ToString( m_Duration ) );
			xml.WriteEndElement();

			xml.WriteStartElement( "comment" );
			xml.WriteString( m_Comentarios );
			xml.WriteEndElement();

			xml.WriteEndElement();
		}

		public void UpdateInfo( string comentarios, TimeSpan duration )
                {
			m_Comentarios = comentarios;
			m_Duration = duration;
                }
	}
}
