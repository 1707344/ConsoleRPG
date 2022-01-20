namespace ConsoleRPG
{
    class AimingIndicator : BaseObject
    {
        Renderer renderer;
        public Position position;
        public Movement.Direction direction;
        public AimingIndicator(Map map, int x, int y, Movement.Direction direction) : base(map)
        {
            switch (direction)
            {
                case Movement.Direction.North:
                    y--;
                    break;
                case Movement.Direction.East:
                    x++;
                    break;
                case Movement.Direction.West:
                    x--;
                    break;
                case Movement.Direction.South:
                    y++;
                    break;
            }
            this.direction = direction;
            renderer = new Renderer(this, '∙', 1, new Color(200, 200, 200), false);
            position = new Position(this, x, y);

        }

        public void SetPosition(int x, int y)
        {
            position.x = x;
            position.y = y;
        }
    }
}
