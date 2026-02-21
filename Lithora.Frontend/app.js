const API = "http://localhost:5157/api";

async function simulate() {
  const res = await fetch(`${API}/simulator/machines/M01`, {
    method: "POST"
  });
  const data = await res.json();
  const outputElement = document.getElementById("output");
  if (outputElement) {
    outputElement.textContent = JSON.stringify(data, null, 2);
  }
}
