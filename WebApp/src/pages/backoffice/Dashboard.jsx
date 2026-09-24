function Dashboard() {
  const fullName = localStorage.getItem("fullName");
  const role = localStorage.getItem("role");

  const handleLogout = () => {
    localStorage.clear();
    window.location.href = "/login";
  };

  return (
    <div className="container mt-5">
      <h3>Dashboard</h3>
      <p>
        Welcome, {fullName} ({role})
      </p>
      <button className="btn btn-secondary" onClick={handleLogout}>
        Logout
      </button>
    </div>
  );
}

export default Dashboard;
