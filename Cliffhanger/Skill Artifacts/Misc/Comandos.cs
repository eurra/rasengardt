// Comandos vinculados a Artefactos de Habilidad.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using System.Reflection;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;
using Server.Gumps;

namespace Server.Scripts.Commands
{
	public class SACommands
	{
		public static readonly int DelaySwitch = 3; // Delay de usar el comando Switch el segundos.

		public static void Initialize()
		{
			EventSink.Speech += new SpeechEventHandler( EventSink_Speech );
			Server.Commands.Register( "ToArtifact", AccessLevel.GameMaster, new CommandEventHandler( ToRaceItem_OnCommand ) );
		}
		
		private static void EventSink_Speech( SpeechEventArgs args )
		{
			if( args.Speech.ToLower() == "cambia artefacto" )
				CheckSwitch( args.Mobile );
		}
		
		private static void CheckSwitch( Mobile m )
		{
                	foreach( Item i in m.Items )
			{
				if( i is SkillArtifact )
				{
					((SkillArtifact)i).Switch( m );
					return;
				}
			}
			
			m.SendMessage( "No tienes ningún artefacto equipado." );
		}

		[Usage( "ToArtifact <tipo>" )]
		[Description( "Transforma un item tradicional a un Artefact de Habilidad" )]
		private static void ToRaceItem_OnCommand( CommandEventArgs e )
		{
			if ( e.Length < 1 )
                        {
                        	e.Mobile.SendMessage("Necesitas especificar un tipo de Artefacto para transformar.");
                        	return;
                        }

                        Type tipo = ScriptCompiler.FindTypeByName( e.GetString( 0 ) );

			SkillArtifactInfo info = SAUtility.BuscarInfo( tipo );

			if( info == null )
                        {
                        	e.Mobile.SendMessage("El artefacto especifcado es inválido.");
                        	return;
                        }

			e.Mobile.Target = new TransformTarget( tipo );
                        e.Mobile.SendMessage( "Selecciona el item que deseas transformar en {0}.", info.Name );
		}

		private class TransformTarget : Target
		{
			private Type m_Type;

			public TransformTarget( Type type ) : base( 15, false, TargetFlags.None )
                	{
                		m_Type = type;
                	}

                	protected override void OnTarget( Mobile from, object targ )
			{
				if( targ is Item )
				{
                                	if( !SAUtility.EsTipoBaseValido( m_Type, targ.GetType() ) )
                                	{
                                        	from.SendMessage( "Ese Item no se puede transformar al artefacto señalado!" );
                                        	return;
                                 	}
                                 	
                                 	object[] args = new object[]{ targ };

					Item newitem = (Item) Activator.CreateInstance( m_Type, args );

					if( newitem != null )
					{
						from.Backpack.DropItem( newitem );
						((Item)targ).Delete();
						from.SendMessage("El Item fue transformado exitosamente, ahora esta en tu bolso.");
     					}
    				}
				else
				{
                                	from.SendMessage("Debes elegir un Item!");
    				}
   			}
		}
	}
}
