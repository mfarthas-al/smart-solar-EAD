import { Navigate } from "react-router-dom";

// Wrap any protected page with this so it redirects to /login when there's
// no saved token. Usage: <Route path="/x" element={<RequireAuth><X /></RequireAuth>} />
function RequireAuth({ children }) {
  const token = localStorage.getItem("token");
  if (!token) {
    return <Navigate to="/login" />;
  }
  return children;
}

export default RequireAuth;
