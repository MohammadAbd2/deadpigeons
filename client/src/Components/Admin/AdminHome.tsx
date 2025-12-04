import Navbar from "../Navbar.tsx";
import Users from "../../api/Users.tsx";
import Admins from "../../api/Admins.tsx";
import Transactions from "../../api/Transactions.tsx";
import AdminBoards from "../../api/AdminBoards.tsx";

export function AdminHome() {
    return (
        <div style={{ height: "100vh", display: "flex", flexDirection: "column" }}>
            {/* Navbar at the top */}
            <Navbar title="Home" />

            {/* Main content area fills remaining height */}
            <div style={{
                display: "grid",
                gridTemplateColumns: "1fr 2fr", // First column is wider
                gap: "20px",
                flex: 1, // Fill the remaining height
                padding: "20px",
                overflowY: "auto" // Allow scrolling if content overflows
            }}>
                {/* First column: Admin Boards and Transactions */}

                <div style={{ display: "flex", flexDirection: "column" }}>
                    <Admins />
                    <Users />
                </div>
                {/* Second column: Users */}
                <div style={{ display: "flex", flexDirection: "column", gap: "20px" }}>
                    <AdminBoards />
                    <Transactions />
                </div>



                {/* Third column: Admins */}
                <div style={{ display: "flex", flexDirection: "column" }}>

                </div>
            </div>
        </div>
    );
}
