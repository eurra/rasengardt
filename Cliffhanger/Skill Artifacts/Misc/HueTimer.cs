// HueTimer.
// Hecho por Cliffhanger (archon.cl@gmail.com)

using System;
using Server;
using Server.Items;

namespace Server.Items
{

	public class HueTimer : Timer
	{
		private Item m_Item;
		private int m_prevHue;
		private int[] m_BaseHues;
		private int index;
		private bool sube;

		public HueTimer( Item item, int[] basehues ) : base( TimeSpan.Zero, TimeSpan.FromSeconds ( 0.25 ) )
		{
			m_Item = item;
			m_prevHue = item.Hue;
			m_BaseHues = basehues;
			index = 0;

			Priority = TimerPriority.TwoFiftyMS;
		}

		protected override void OnTick()
		{
			if( m_BaseHues == null || m_BaseHues.Length < 2 || m_Item == null || m_Item.Deleted )
				Stop();

			if( index == 0 )
			{
                        	m_Item.Hue = m_BaseHues[1];
                                index = 1;
                                sube = true;
                        }
                        else if( index == m_BaseHues.Length - 1 )
                        {
                        	m_Item.Hue = m_BaseHues[m_BaseHues.Length - 2];
                                index = m_BaseHues.Length - 2;
                                sube = false;
                        }
                        else
                        {
                        	if( sube )
                        	{
                        		m_Item.Hue = m_BaseHues[index + 1];
					index++;
				}
				else
				{
                        		m_Item.Hue = m_BaseHues[index - 1];
					index--;
				}
                        }
                }

	}
}
