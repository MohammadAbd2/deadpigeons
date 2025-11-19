import { ApiClient, User } from "../api/apiClient";
import { useEffect, useState } from "react";

function Users() {

    const [users, setUsers] = useState<User[]>([]);

    useEffect(() => {
        const client = new ApiClient("http://localhost:5139");

        client.usersAll()
            .then((data: User[]) => {
                setUsers(data);
            })
            .catch((err: unknown) => {
                if (err instanceof Error) {
                    console.error("API error:", err.message);
                } else {
                    console.error("API error:", err);
                }
            });
    }, []);

    return (
        <>
            <h1>Users</h1>
            <ul>
                {users.map((u: User) => (
                    <li key={u.id}>{u.name}</li>
                    ))}
            </ul>
        </>
    );
}

export default Users;
