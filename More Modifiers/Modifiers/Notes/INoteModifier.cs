namespace More_Modifiers.Modifiers.Notes
{
    interface INoteModifier
    {
        void Cleanup();

        bool Enabled { get; set; }
    }
}
