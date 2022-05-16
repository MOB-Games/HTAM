public class Path
{
    public readonly PathInfo Info;
    public readonly EnemySpawner EnemySpawner;

    public Path(PathInfo info, EnemySpawner enemySpawner)
    {
        Info = info;
        EnemySpawner = enemySpawner;
    }
}
