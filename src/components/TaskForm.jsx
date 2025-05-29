// components/TaskForm.jsx
import { useState } from "react";

export default function TaskForm({ onAdd }) {
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [date, setDate] = useState("");

    const handleSubmit = (e) => {
        e.preventDefault();
        if (title && date) {
            onAdd({ title, description, date });
            setTitle("");
            setDescription("");
            setDate("");
        }
    };

    return (
        <form onSubmit={handleSubmit} className="task-form">
            <input type="text" placeholder="Название задачи" value={title} onChange={(e) => setTitle(e.target.value)} required />
            <input type="text" placeholder="Описание" value={description} onChange={(e) => setDescription(e.target.value)} />
            <input type="date" value={date} onChange={(e) => setDate(e.target.value)} required />
            <button type="submit">Добавить задачу</button>
        </form>
    );
}
