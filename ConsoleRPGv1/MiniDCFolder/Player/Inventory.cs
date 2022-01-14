using System;

namespace ConsoleRPG
{
    public static class Inventory
    {
        public static int numberOfFreezingTraps = 2;
        static Inventory()
        {
            InputHandler.AddListener(new KeyListener(PressedZ, ConsoleKey.Z));

        }

        public static void DisplayInventory()
        {
            Console.WriteLine($"Freezing Traps: {numberOfFreezingTraps} (Z)");
        }

        public static bool PressedZ()
        {
            if (numberOfFreezingTraps > 0)
            {
                numberOfFreezingTraps--;
                MiniDC.player.GetMap().addingObjecsFuncs.Add(PlaceFreezingTrap);
            }
            return true;
        }
        public static bool PlaceFreezingTrap()
        {
            Player player = MiniDC.player;
            player.GetMap().objects.Add(new FreezingTrap(player.GetMap(), player.GetComponent<Position>().x, player.GetComponent<Position>().y));
            return true;
        }
    }
}
