const API = "http://localhost:5157/api";

async function simulate() {
  const res = await fetch(`${API}/simulator/machines/M01`, {
    method: "POST"
  });
  const data = await res.json();
  document.getElementById("output")!.textContent =
    JSON.stringify(data, null, 2);
}
