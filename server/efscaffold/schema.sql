DROP SCHEMA IF EXISTS deadpigeons CASCADE;
CREATE SCHEMA IF NOT EXISTS deadpigeons;

-- ======================
-- Users Table
-- ======================
CREATE TABLE deadpigeons.Users
(
    id        text PRIMARY KEY NOT NULL,
    name      text NOT NULL,
    phone     text NOT NULL,
    email     text NOT NULL,
    password  text NOT NULL,
    balance   int NOT NULL,
    isActive  boolean NOT NULL
);

-- ======================
-- Boards Table
-- ======================
CREATE TABLE deadpigeons.Boards
(
    id             text PRIMARY KEY NOT NULL,
    name           text NOT NULL,
    weekNumber     int NOT NULL,              
    totalWinners   int NOT NULL,
    winningNumbers text NOT NULL,
    winningUsers   text NOT NULL,
    isOpen         boolean NOT NULL
);

-- ======================
-- Admins Table
-- ======================
CREATE TABLE deadpigeons.Admins
(
    id        text PRIMARY KEY NOT NULL,
    name      text NOT NULL,
    email     text NOT NULL,
    password  text NOT NULL
);

-- ======================
-- Transactions Table
-- ======================
CREATE TABLE deadpigeons.Transactions
(
    id             text PRIMARY KEY NOT NULL,
    username       text NOT NULL,
    transactionId  text NOT NULL,
    status         int NOT NULL,
    balance        int NOT NULL
);
