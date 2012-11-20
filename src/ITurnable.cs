namespace mtBot
{
    /* NB: It's only valid to advance an object 1 turn at a time. The next turn must then be taken under a new GameState; individual objects can't, as a rule, be advanced >1 time without consideration for the state of the universe. */
    interface ITurnable<T>
    {
        /* Generate new T with state for the following turn under GameState gs */
        T Turn( GameState gs );
    }
}
