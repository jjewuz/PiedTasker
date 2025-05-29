// components/TaskList.jsx
export default function TaskList({ tasks, onToggle, onDelete }) {
    return (
        <ul className="task-list">
            {tasks.map((task) => (
                <li key={task.id} style={{ textDecoration: task.done ? "line-through" : "none" }}>
                    <strong>{task.title}</strong> — {task.date}
                    <button onClick={() => onToggle(task.id)}>{task.done ? "Отменить" : "Выполнено"}</button>
                    <button onClick={() => onDelete(task.id)}>Удалить</button>
                </li>
            ))}
        </ul>
    );
}
