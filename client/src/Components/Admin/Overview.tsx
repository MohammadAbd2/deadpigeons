import { useEffect, useState } from "react";
import Navbar from "../Navbar";
import { User } from "../../api/apiClient.ts";
import { Board } from "../../api/apiClient.ts";
import {ApiClient} from "../../api/apiClient.ts"; // adjust to your structure
import {finalUrl} from "../../baseUrl.ts";
// --- TYPES USED JUST FOR UI ---
type PlayerOverview = {
    user: User;
    boardCount: number;
    winningBoards: number;
};

export function Overview() {
    const [users, setUsers] = useState<User[]>([]);
    const [boards, setBoards] = useState<Board[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const client = new ApiClient(finalUrl);

        Promise.all([client.usersAll(), client.boardsAll()])
            .then(([u, b]) => {
                setUsers(u);
                setBoards(b);
            })
            .finally(() => setLoading(false));
    }, []);

    if (loading) {
        return (
            <>
                <Navbar title="Overview" />
                <div className="flex justify-center mt-10">
                    <span className="loading loading-lg"></span>
                </div>
            </>
        );
    }

    // ---- DERIVED UI DATA ----

    // Group boards by week for Game History
    const gameHistory = boards
        .map((b) => ({
            weekNumber: b.weeknumber ? new Date(b.weeknumber).toLocaleTimeString() : "?",
            winningNumbers: b.winningnumbers
                ? b.winningnumbers.split(",").map((n) => Number(n.trim()))
                : [],
            totalWinningBoards: b.totalwinners ?? 0,
            isOpen: b.isopen ?? false,
        }))
        .sort((a, b) => Number(b.weekNumber) - Number(a.weekNumber));

    // For each user, count boards + count winners
    const userOverview: PlayerOverview[] = users.map((u) => {
        const boardsByUser = boards.filter((b) =>
            b.winningusers?.toLowerCase().includes(u.name?.toLowerCase() ?? "")
        );

        const winningBoards = boardsByUser.filter((b) => b.totalwinners && b.totalwinners > 0).length;

        return {
            user: u,
            boardCount: boardsByUser.length,
            winningBoards,
        };
    });

    return (
        <>
            <Navbar title="Overview" />

            {/* --- GAME HISTORY SECTION --- */}
            <div className="m-5 p-5 rounded-xl bg-base-200">
                <h2 className="text-2xl font-bold mb-4 text-center">Game History</h2>

                <div className="overflow-x-auto">
                    <table className="table">
                        <thead>
                        <tr>
                            <th>Week</th>
                            <th>Winning Numbers</th>
                            <th>Total Winner Boards</th>
                            <th>Status</th>
                        </tr>
                        </thead>
                        <tbody>
                        {gameHistory.map((g, idx) => (
                            <tr key={idx} className="hover:bg-base-300">
                                <td>{g.weekNumber}</td>
                                <td>
                                    {g.winningNumbers.length > 0
                                        ? g.winningNumbers.join(", ")
                                        : "-"}
                                </td>
                                <td>{g.totalWinningBoards}</td>
                                <td>{g.isOpen ? "open" : "closed"}</td>
                            </tr>
                        ))}
                        </tbody>
                    </table>
                </div>
            </div>

            {/* --- PLAYER PARTICIPATION SECTION --- */}
            <div className="m-5 p-5 rounded-xl bg-base-200">
                <h2 className="text-2xl font-bold mb-4 text-center">Player Participation Overview</h2>


                <div className="overflow-x-auto">
                    <table className="table">
                        <thead>
                        <tr>
                            <th>Player</th>
                            <th>Email</th>
                            <th>Phone</th>
                            <th>Status</th>
                            <th># Boards</th>
                            <th>Winning Boards</th>
                        </tr>
                        </thead>
                        <tbody>
                        {userOverview.map((p) => (
                            <tr key={p.user.id} className="hover:bg-base-300">
                                <td>{p.user.name}</td>
                                <td>{p.user.email}</td>
                                <td>{p.user.phone}</td>
                                <td>{p.user.isactive ? "active" : "inactive"}</td>
                                <td>{p.boardCount}</td>
                                <td>{p.winningBoards}</td>
                            </tr>
                        ))}
                        </tbody>
                    </table>
                </div>
            </div>

            {/* --- BOARDS PER PLAYER SECTION --- */}
            <div className="m-5 mt-10">
                <h2 className="text-2xl font-bold mb-4 text-center">Boards Per Player</h2>

                {users.map((u) => {
                    const boardsByUser = boards.filter((b) =>
                        b.winningusers?.toLowerCase().includes(u.name?.toLowerCase() ?? "")
                    );

                    return (
                        <div key={u.id} className="mb-8 p-5 bg-base-200 rounded-xl">
                            <h3 className="text-xl font-semibold mb-2">
                                {u.name} — {boardsByUser.length} board(s)
                            </h3>

                            {boardsByUser.length === 0 && (
                                <p className="text-sm opacity-70">No boards this week.</p>
                            )}

                            {boardsByUser.length > 0 && (
                                <div className="overflow-x-auto">
                                    <table className="table">
                                        <thead>
                                        <tr>
                                            <th>Board ID</th>
                                            <th>Week</th>
                                            <th>Winning Numbers</th>
                                            <th>Winner?</th>
                                        </tr>
                                        </thead>
                                        <tbody>
                                        {boardsByUser.map((b) => (
                                            <tr key={b.id} className="hover:bg-base-300">
                                                <td>{b.id}</td>
                                                <td>
                                                    {b.weeknumber
                                                        ? new Date(b.weeknumber).toLocaleTimeString()
                                                        : "-"}
                                                </td>
                                                <td>{b.winningnumbers || "-"}</td>
                                                <td>{b.totalwinners && b.totalwinners > 0 ? "YES" : "NO"}</td>
                                            </tr>
                                        ))}
                                        </tbody>
                                    </table>
                                </div>
                            )}
                        </div>
                    );
                })}
            </div>
        </>
    );
}