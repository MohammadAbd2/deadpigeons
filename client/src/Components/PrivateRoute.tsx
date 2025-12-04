// PrivateRoute.tsx
import type { ReactNode } from "react";
import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "./UserRole";

interface PrivateRouteProps {
    allowedRoles: ("admin" | "user")[];
    children: ReactNode;
}

const PrivateRoute = ({ allowedRoles, children }: PrivateRouteProps) => {
    const { role } = useAuth();
    const location = useLocation(); // to remember attempted URL

    // If no user is logged in or role is not allowed
    if (!role || !allowedRoles.includes(role)) {
        // Redirect to AuthFailed page and keep attempted URL in state
        return <Navigate to="/AuthFaild" replace state={{ from: location }} />;
    }

    // User is authorized
    return <>{children}</>;
};

export default PrivateRoute;
