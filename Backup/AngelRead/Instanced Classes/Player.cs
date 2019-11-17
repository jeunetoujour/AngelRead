﻿using MemoryLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AngelRead
{
    /// <summary>Contains information about player.</summary>
    public class Player : Entity
    {
        /// <summary>Current spell being cast by player.  If 0, no spell is being cast, else value is spell ID.</summary>
        public byte Spell;
        /// <summary>Current experience.</summary>
        public int XP;
        /// <summary>Total experience needed for next level.</summary>
        public int MaxXP;
        /// <summary>Player's current HP.</summary>
        new public int Health;
        /// <summary>Player's maximum HP</summary>
        public int MaxHealth;
        /// <summary>Player's current mana.</summary>
        public int MP;
        /// <summary>Player's maximum mana.</summary>
        public int MaxMP;
        /// <summary>Maximum number of slots in all cubes.</summary>
        public int MaxSlots;
        public int Kinah;
        public int PtrKinah;
        public bool InventoryFull;
        public int SelfPtr;
        public int HasTarget;
        /// <summary>Players's current stance.</summary> 
        new public eStance Stance;

        /// <summary>
        /// Class instance initializer.
        /// </summary>
        public Player()
        {
            Updatenamelvl();
            this.Update();
        }
        public void getpcptr()
        {
            EntityList elist = new EntityList();
            elist.Update();
            foreach (Entity thing in elist)
            {
                if (thing.Name == this.Name)
                {
                    if (thing._PtrEntity != 0)
                    {
                        SelfPtr = thing._PtrEntity;
                    }
                    else SelfPtr = thing.PtrEntity;
                }
            }
        }

        public void Updatenamelvl()
        {
            this.Name = Memory.ReadString(Process.handle, (Process.Modules.Game + 0xA699A8 + 8), 32, true);
            this.Level = Memory.ReadByte(Process.handle, (Process.Modules.Game + 0xA32988 + 8));
            this.ID = (int)Memory.ReadUInt(Process.handle, (uint)(Process.Modules.Game + 0xA27EB8 + 8));//A1CC58//0xA1CC5C
            getpcptr();
            this.Class = (eClass)(int)Memory.ReadUInt(Process.handle, (uint)(this.SelfPtr + 0x19C + 8));
        }

        public int GetUsedSpace()
        {
            int UsedSpace = 0;
            int InventoryPtr = Memory.ReadInt(Process.handle, (Process.Modules.Game + 0xA68650 + 8)); //A68650
            int cubeID = 0x2E8;
            int MaxSlots = GetMaxCubes();

            int currentItem = 0x0;
            int Offset2 = (int)Memory.ReadUInt(Process.handle, (uint)(InventoryPtr + cubeID));
            int InventoryList = (int)Memory.ReadUInt(Process.handle, (uint)(Offset2 + 0x298)); //0x298

            for (int slot = 0; slot < MaxSlots; slot++)
            {
                currentItem = currentItem + 0x4;
                int thisItem = (int)Memory.ReadUInt(Process.handle, (uint)(InventoryList + currentItem));
                int thisItemID = (int)Memory.ReadUInt(Process.handle, (uint)(thisItem + 0x98));
                int SlotID = (int)Memory.ReadUInt(Process.handle, (uint)(thisItem + 0x8C));

                if (thisItemID != 0 && SlotID >= 0) UsedSpace++;
                
                if ((slot+1) % 27 == 0)
                {
                    currentItem = 0;
                    cubeID += 4;
                    Offset2 = (int)Memory.ReadUInt(Process.handle, (uint)(InventoryPtr + cubeID));
                    InventoryList = (int)Memory.ReadUInt(Process.handle, (uint)(Offset2 + 0x298));
                }
            }
            
            return UsedSpace;
        }

        public int GetMaxCubes()
        {
            return Memory.ReadInt(Process.handle, (Process.Modules.Game + 0xA329F0 + 8));//A2F9F0
        }

        public bool CubeFull()
        {
            if (GetMaxCubes() == GetUsedSpace()) return true; //Until fixed have false
            else return false;
        }

        public void Updateafterkill()
        {
            this.PtrKinah = (int)Memory.ReadUInt(Process.handle, (uint)(Process.Modules.Game + 0xa2739c + 8));
            this.Kinah = (int)Memory.ReadUInt(Process.handle, (uint)(PtrKinah + 0x138));
            this.XP = Memory.ReadInt(Process.handle, (Process.Modules.Game + 0xA329A0 + 8));//A2f9A0
            this.MaxXP = Memory.ReadInt(Process.handle, (Process.Modules.Game + 0xA32990 + 8));//A2F990
        }

        new public void UpdateRot()
        {
            this.Rotation = Memory.ReadFloat(Process.handle, (Process.Modules.Game + 0xA27B28 + 8));//A24B28
            System.Threading.Thread.Sleep(10);
        }
        
        new public void Update()
        {
            this.Spell = Memory.ReadByte(Process.handle, (Process.Modules.Game + 0xA28730 + 8)); //A25730
            this.Health = Memory.ReadInt(Process.handle, (Process.Modules.Game + 0xA329B0 + 8));//A2F9b0
            this.MaxHealth = Memory.ReadInt(Process.handle, (Process.Modules.Game + 0xA329AC + 8));
            this.MP = Memory.ReadInt(Process.handle, (Process.Modules.Game + 0xA329B8 + 8));
            this.MaxMP = Memory.ReadInt(Process.handle, (Process.Modules.Game + 0xA329B4 + 8));  
            this.X = Memory.ReadFloat(Process.handle, (Process.Modules.Game + 0xA2A3C0 + 8));
            this.Y = Memory.ReadFloat(Process.handle, (Process.Modules.Game + 0xA2A3C4 + 8));
            this.Z = Memory.ReadFloat(Process.handle, (Process.Modules.Game + 0xA2A3C8 + 8));
        }
    }
}
