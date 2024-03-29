﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MemoryLib;

namespace AngelRead
{
    public class AbilityList: IEnumerable<Ability>
    {
        Dictionary<string, Ability> abilities;
        HashSet<int> found;

        private void Recurse(Ability ability)
        {
            if (!found.Add(ability.PtrBase))
                return;

            if (ability.AbilityName != "" && !abilities.ContainsKey(ability.AbilityName))
              abilities.Add(ability.AbilityName, ability);
            Recurse(new Ability(ability.PtrChild1));
            Recurse(new Ability(ability.PtrChild3));
            Recurse(new Ability(ability.PtrChild2));
        }
        
        public void Update()
        {
            found = new HashSet<int>();
            abilities = new Dictionary<string, Ability>();

            int headPtr = (int)Memory.ReadUInt(Process.handle, (uint)(Process.Modules.Game + 0xA2739C + 8));//A2439C
            headPtr = (int)Memory.ReadUInt(Process.handle, (uint)(headPtr + 0x1e4));

            Recurse(new Ability(headPtr));
        }

        public Ability this[string AbilityName]
        {

            get
            {
                try
                {
                    return abilities[AbilityName];
                }
                catch (Exception abil) { MessageBox.Show("Ability: " + AbilityName + " " + abil); return abilities[AbilityName];}
            }
            
        }
     

        #region IEnumerable<Ability> Members

        public IEnumerator<Ability> GetEnumerator()
        {
            return abilities.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
