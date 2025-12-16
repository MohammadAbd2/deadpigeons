import { atomWithStorage } from "jotai/utils";

export type User = {
    userID : string;
    username: string;
    email: string;
    phone?: string;
    role: "User" | "Admin";
    balance?: number;
    isActive : boolean;
} | null;

// This atom will automatically persist in localStorage under the key "user"
export const userAtom = atomWithStorage<User>("User", null);


