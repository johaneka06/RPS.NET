CREATE TABLE "player"
(
    playerId        UUID PRIMARY KEY,
    playername      VARCHAR(255) NOT NULL,
    created_at      TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
    updated_at      TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE "exp"
(
    expId       UUID PRIMARY KEY,
    playerId    UUID NOT NULL,
    xpValue     INT NOT NULL,
    created_at  TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (playerId) REFERENCES "player"(playerId)
);

CREATE TABLE "room"
(
    roomId      UUID PRIMARY KEY,
    maxPlayer   INT NOT NULL,
    created_at  TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
    updated_at  TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
    deleted_at  TIMESTAMPTZ
);

CREATE TABLE "game"
(
    gameId      UUID PRIMARY KEY,
    roomId      UUID NOT NULL,
    game_name   VARCHAR(255) NOT NULL,
    created_at  TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
    deleted_at  TIMESTAMPTZ,
    FOREIGN KEY (roomId) REFERENCES "room"(roomId)
)

CREATE TABLE "play"
(
    roomId      UUID NOT NULL,
    userId      UUID NOT NULL,
    created_at  TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
    deleted_at  TIMESTAMPTZ,
    FOREIGN KEY (roomid) REFERENCES "room"(roomId),
    FOREIGN KEY (userId) REFERENCES "user"(userId)
);

CREATE TABLE "action"
(
    actionId    UUID PRIMARY KEY,
    roomId      UUID NOT NULL,
    result      JSONB NOT NULL,
    state       JSONB NOT NULL,
    created_at  TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (roomId) REFERENCES "room"(roomId)
);

CREATE TABLE "exp_snapshot"
(
    snapshotId      UUID PRIMARY KEY,
    lastId          UUID NOT NULL,
    playerId        UUID NOT NULL,
    expValue        INT NOT NULL,
    created_at      TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
    lastSnapshotAt  TIMESTAMPTZ NOT NULL,
    FOREIGN KEY (playerId) REFERENCES "player"(playerId)
);