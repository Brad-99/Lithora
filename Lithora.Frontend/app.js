const API = "http://localhost:5157/api";

const statusEl = () => document.getElementById("status");
const showStatus = (msg) => (statusEl().textContent = msg ?? "");

async function fetchJson(url, options = {}) {
  const res = await fetch(url, options);
  if (!res.ok) {
    const body = await res.text();
    throw new Error(`HTTP ${res.status}: ${body || res.statusText}`);
  }
  return res.json();
}

// Simulate run
async function simulate() {
  const machine = document.getElementById("sim-machine").value || "M01";
  const out = document.getElementById("simulate-output");
  showStatus("Simulating...");
  try {
    const data = await fetchJson(`${API}/simulator/machines/${machine}`, {
      method: "POST"
    });
    out.textContent = JSON.stringify(data, null, 2);
    // refresh list to show new inspection
    await loadInspections();
    showStatus("Simulation complete.");
  } catch (err) {
    out.textContent = `Request failed: ${err}`;
    showStatus("");
    console.error(err);
  }
}

// Manual create inspection
async function createInspection() {
  const machineId = document.getElementById("ins-machine").value;
  const photoLot = document.getElementById("ins-photolot").value || null;
  const result = parseInt(document.getElementById("ins-result").value, 10);
  showStatus("Creating inspection...");
  try {
    await fetchJson(`${API}/inspections`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        machineId,
        photoLot,
        inspectedAt: null,
        result
      })
    });
    await loadInspections();
    showStatus("Inspection created.");
  } catch (err) {
    showStatus(`Create failed: ${err}`);
  }
}

// Load inspections with filters
async function loadInspections() {
  const machine = document.getElementById("filter-machine").value;
  const from = document.getElementById("filter-from").value;
  const to = document.getElementById("filter-to").value;

  const params = new URLSearchParams();
  if (machine) params.append("machineId", machine);
  if (from) params.append("from", from);
  if (to) params.append("to", to);

  const tableHost = document.getElementById("inspections-table");
  tableHost.textContent = "Loading...";
  try {
    const data = await fetchJson(`${API}/inspections?${params.toString()}`);
    tableHost.innerHTML = renderInspectionsTable(data);
    // click row to set selected inspection id
    document.querySelectorAll("[data-inspection-id]").forEach((row) => {
      row.addEventListener("click", () => {
        const id = row.getAttribute("data-inspection-id");
        document.getElementById("defect-inspection-id").value = id;
        loadDefects();
      });
    });
  } catch (err) {
    tableHost.textContent = `Load failed: ${err}`;
  }
}

function renderInspectionsTable(rows) {
  if (!rows.length) return "<p>No inspections found.</p>";
  const trs = rows
    .map(
      (r) => `<tr data-inspection-id="${r.id}">
        <td>${r.id}</td>
        <td>${r.machineId}</td>
        <td>${r.photoLot ?? ""}</td>
        <td>${new Date(r.inspectedAt).toISOString()}</td>
        <td><span class="badge ${r.result === 0 ? "pass" : "fail"}">${r.result === 0 ? "Pass" : "Fail"}</span></td>
      </tr>`
    )
    .join("");
  return `<table>
    <thead><tr><th>Id</th><th>Machine</th><th>PhotoLot</th><th>InspectedAt (UTC)</th><th>Result</th></tr></thead>
    <tbody>${trs}</tbody>
  </table>`;
}

// Stats
async function loadStats() {
  const machine = document.getElementById("stats-machine").value;
  const days = document.getElementById("stats-days").value || 7;
  const out = document.getElementById("stats-output");
  out.textContent = "Loading...";
  try {
    const data = await fetchJson(
      `${API}/stats/fail-rate?machineId=${encodeURIComponent(
        machine
      )}&days=${days}`
    );
    out.textContent = JSON.stringify(data, null, 2);
  } catch (err) {
    out.textContent = `Failed: ${err}`;
  }
}

// Defects
async function loadDefects() {
  const inspectionId = document.getElementById("defect-inspection-id").value;
  const host = document.getElementById("defects-table");
  if (!inspectionId) {
    host.textContent = "Select an inspection first.";
    return;
  }
  host.textContent = "Loading...";
  try {
    const data = await fetchJson(
      `${API}/inspections/${inspectionId}/defects`
    );
    host.innerHTML = renderDefectsTable(data);
  } catch (err) {
    host.textContent = `Failed: ${err}`;
  }
}

function renderDefectsTable(rows) {
  if (!rows.length) return "<p>No defects.</p>";
  const trs = rows
    .map(
      (d) =>
        `<tr><td>${d.id}</td><td>${d.defectType}</td><td>${d.severity}</td><td>${d.description ?? ""}</td></tr>`
    )
    .join("");
  return `<table>
    <thead><tr><th>Id</th><th>Type</th><th>Severity</th><th>Description</th></tr></thead>
    <tbody>${trs}</tbody>
  </table>`;
}

async function addDefect() {
  const inspectionId = document.getElementById("defect-inspection-id").value;
  if (!inspectionId) {
    showStatus("Please select an inspection first.");
    return;
  }
  const defectType = document.getElementById("def-type").value;
  const severity = parseInt(document.getElementById("def-severity").value, 10);
  const description = document.getElementById("def-desc").value || null;

  showStatus("Adding defect...");
  try {
    await fetchJson(`${API}/inspections/${inspectionId}/defects`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ defectType, severity, description })
    });
    await loadDefects();
    showStatus("Defect added.");
  } catch (err) {
    showStatus(`Add defect failed: ${err}`);
  }
}

function wireEvents() {
  document.getElementById("btn-simulate").onclick = simulate;
  document.getElementById("btn-create-inspection").onclick = createInspection;
  document.getElementById("btn-refresh").onclick = loadInspections;
  document.getElementById("btn-stats").onclick = loadStats;
  document.getElementById("btn-load-defects").onclick = loadDefects;
  document.getElementById("btn-add-defect").onclick = addDefect;
}

window.addEventListener("DOMContentLoaded", async () => {
  wireEvents();
  await loadInspections();
  await loadStats();
});
