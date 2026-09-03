DROP TABLE IF EXISTS test;

CREATE TABLE IF NOT EXISTS Watches (
    Id              TEXT PRIMARY KEY,
    Name            TEXT NOT NULL,
    ModelNumber     TEXT NOT NULL,
    CaseSize        INTEGER,
    CaseShape       INTEGER NOT NULL,
    CaseMaterial    INTEGER NOT NULL,
    MovementType    INTEGER NOT NULL,
    WatchComplication INTEGER NOT NULL,
    Style           TEXT NOT NULL,
    OriginalPrice   NUMERIC,
    Gender          INTEGER NOT NULL,
    ReleaseYear     TEXT,
    Description     TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS WatchBraceletTypes (
    WatchId         TEXT NOT NULL REFERENCES Watches(Id) ON DELETE CASCADE,
    BraceletType    INTEGER NOT NULL,
    PRIMARY KEY (WatchId, BraceletType)
);

CREATE TABLE IF NOT EXISTS WatchPictures (
    Id              TEXT PRIMARY KEY,
    WatchId         TEXT NOT NULL REFERENCES Watches(Id) ON DELETE CASCADE,
    Url             TEXT NOT NULL,
    Width           INTEGER NOT NULL DEFAULT 0,
    Height          INTEGER NOT NULL DEFAULT 0
);

CREATE INDEX IF NOT EXISTS IX_Watches_ModelNumber ON Watches(ModelNumber);
CREATE INDEX IF NOT EXISTS IX_Watches_Name ON Watches(Name);
