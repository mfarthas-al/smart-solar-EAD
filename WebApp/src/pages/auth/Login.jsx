import { useState } from "react";
import api from "../../api/client";

function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    try {
      const response = await api.post("/auth/login", { email, password });

      // save login info so the user stays logged in after refresh
      localStorage.setItem("token", response.data.token);
      localStorage.setItem("role", response.data.role);
      localStorage.setItem("fullName", response.data.fullName);

      // full page redirect (not client-side navigate) so the app reloads
      // and reads the saved token fresh
      window.location.href = "/dashboard";
    } catch (err) {
      if (err.response) {
        // server responded, but rejected the login (wrong email/password, inactive account, etc.)
        setError(err.response.data.message || "Invalid email or password");
      } else {
        // request never reached the server
        setError("Could not reach the server. Is the WebService running?");
      }
    }
  };

  return (
    <div className="container mt-5" style={{ maxWidth: "400px" }}>
      <h3 className="mb-4">Login</h3>

      {error && <div className="alert alert-danger">{error}</div>}

      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <label className="form-label">Email</label>
          <input
            type="email"
            className="form-control"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>

        <div className="mb-3">
          <label className="form-label">Password</label>
          <input
            type="password"
            className="form-control"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>

        <button type="submit" className="btn btn-primary w-100">
          Login
        </button>
      </form>
    </div>
  );
}

export default Login;
