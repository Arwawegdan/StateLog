﻿namespace StateLog.Shared;
    public class Mapper : BaseEntity
    {
        public string? SchemaName { get; set; }
        public string? UpdatedColoumn { get; set; }
        public DateTime DateTime { get; set; }
        public ChangedColumnType ChangedColumnType { get; set; }
        public string? ChangedColumnNewValue { get; set; }
        public Guid ColoumnId { get; set; }
    }