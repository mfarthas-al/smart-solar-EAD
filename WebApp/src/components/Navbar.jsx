function Navbar() {
  const fullName = localStorage.getItem("fullName");
  const role = localStorage.getItem("role");

  const handleLogout = () => {
    localStorage.clear();
    window.location.href = "/login";
  };

  return (
    <nav className="navbar navbar-dark bg-dark px-3">
      <span className="navbar-brand mb-0">Smart Solar Microgrid</span>
      <div className="d-flex align-items-center">
        <span className="text-light me-3">
          {fullName} ({role})
        </span>
        <button className="btn btn-outline-light btn-sm" onClick={handleLogout}>
          Logout
        </button>
      </div>
    </nav>
  );
}

export default Navbar;
