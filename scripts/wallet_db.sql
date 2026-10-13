-- ============================================
-- WALLETS
-- ============================================

CREATE TABLE wallets (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    balance NUMERIC(19, 2) NOT NULL DEFAULT 0.00,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP NOT NULL
);

-- ============================================
-- TRANSACTIONS
-- ============================================

CREATE TABLE transactions (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    wallet_origin_id UUID NOT NULL,
    wallet_destiny_id UUID NULL,
    type VARCHAR(20) NOT NULL,
    amount NUMERIC(19, 2) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_transaction_wallet_origin
        FOREIGN KEY (wallet_origin_id)
        REFERENCES wallets(id),

    CONSTRAINT fk_transaction_wallet_destiny
        FOREIGN KEY (wallet_destiny_id)
        REFERENCES wallets(id),

    CONSTRAINT chk_transaction_amount
        CHECK (amount > 0),

    CONSTRAINT chk_transaction_type
        CHECK (type IN ('DEPOSIT', 'WITHDRAW', 'TRANSFER'))
);

