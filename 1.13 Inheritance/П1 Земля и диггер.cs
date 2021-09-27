namespace Digger
{
    public class Terrain : ICreature
    {
        public string GetImageFileName() => "Terrain.png";

        public int GetDrawingPriority() => 100;

        public CreatureCommand Act(int x, int y)
            => new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = this };

        public bool DeadInConflict(ICreature conflictedObject)
            => conflictedObject is Player;
    }

    public class Player : ICreature
    {
        public string GetImageFileName() => "Digger.png";
        
        public int GetDrawingPriority() => 0;

        public CreatureCommand Act(int x, int y)
        {
            var command = new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = this };
            switch (Game.KeyPressed)
            {
                case System.Windows.Forms.Keys.Up:
                    command.DeltaY = -1;
                    break;
                case System.Windows.Forms.Keys.Down:
                    command.DeltaY = 1;
                    break;
                case System.Windows.Forms.Keys.Left:
                    command.DeltaX = -1;
                    break;
                case System.Windows.Forms.Keys.Right:
                    command.DeltaX = 1;
                    break;
            }
            
            if (!CanGoTo(x + command.DeltaX, y + command.DeltaY))
            {
                command.DeltaX = 0;
                command.DeltaY = 0;
            }
            return command;
        }

        public bool CanGoTo(int x, int y)
        {
        	return !(x < 0 || y < 0 || x >= Game.MapWidth || y >= Game.MapHeight);
        }

        public bool DeadInConflict(ICreature conflictedObject)
            => false;
    }
}
