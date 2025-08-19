﻿namespace Logs.Migrations
{
    // Reference: https://doi.org/10.3109/00365529709011203
    public enum BristolStoolScale
    {
        Type1 = 1, Type2, Type3, Type4, Type5, Type6, Type7
    }
    public class Entry
    {
        #region Constructors
        public Entry(Guid userId, DateOnly date, TimeOnly time, BristolStoolScale bristolStoolScale)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Date = date;
            Time = time;
            BristolStoolScale = bristolStoolScale;
        }
        #endregion

        #region Properties
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public BristolStoolScale BristolStoolScale { get; set; }
        public string? Notes { get; set; } = null;
        #endregion

        #region Methods
        public override string ToString() => System.Text.Json.JsonSerializer.Serialize(this);
        #endregion
    }
}