import { Routes, Route, Navigate } from "react-router-dom";
import Login from "./pages/auth/Login";
import RequireAuth from "./components/RequireAuth";
import BackofficeDashboard from "./pages/backoffice/Dashboard";
import OperatorDashboard from "./pages/operator/Dashboard";

// Where "/" sends a visitor once we know if they're logged in and what role they are.
function HomeRedirect() {
  const token = localStorage.getItem("token");
  const role = localStorage.getItem("role");

  if (!token) return <Navigate to="/login" />;
  if (role === "GridOperator") return <Navigate to="/operator/dashboard" />;
  return <Navigate to="/backoffice/dashboard" />;
}

function App() {
  return (
    <Routes>
      <Route path="/login" element={<Login />} />

      <Route
        path="/backoffice/dashboard"
        element={
          <RequireAuth>
            <BackofficeDashboard />
          </RequireAuth>
        }
      />

      <Route
        path="/operator/dashboard"
        element={
          <RequireAuth>
            <OperatorDashboard />
          </RequireAuth>
        }
      />

      <Route path="/" element={<HomeRedirect />} />
    </Routes>
  );
}

export default App;
