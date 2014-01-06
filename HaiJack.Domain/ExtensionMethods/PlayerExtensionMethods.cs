using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaiJack.Domain.ExtensionMethods
{
    public static class PlayerExtensionMethods
    {
        public static void ClearHandsAndReset(this List<Hand> hands)
        {
            hands.RemoveAll(h => h.Active == false);
            hands.Single(h => h.Active).ClearCardsAndStatus();
        }
    }
}
