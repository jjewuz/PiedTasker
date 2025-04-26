const API_URL = "https://localhost:5298/api"; // поменяй порт, если другой

export async function fetchTasks() {
    const res = await fetch(`${API_URL}/tasks`);
    return res.json();
}

export async function addTask(task) {
    const res = await fetch(`${API_URL}/tasks`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(task),
    });
    return res.json();
}

export async function deleteTask(id) {
    await fetch(`${API_URL}/tasks/${id}`, {
        method: "DELETE",
    });
}
