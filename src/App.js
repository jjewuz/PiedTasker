import React, { useState, useEffect } from 'react';
import './App.css';

function App() {
    const [tasks, setTasks] = useState([]);
    const [habits, setHabits] = useState([]);
    const [taskTitle, setTaskTitle] = useState('');
    const [taskDate, setTaskDate] = useState('');
    const [habitName, setHabitName] = useState('');
    const [habitFrequency, setHabitFrequency] = useState('ежедневно');

    const userId = localStorage.getItem("tg_id");

    useEffect(() => {
        fetch(`https://justnotes.xyz/api/tasks?userId=${userId}`)
            .then(res => res.json())
            .then(setTasks);

        fetch(`https://justnotes.xyz/api/habits?userId=${userId}`)
            .then(res => res.json())
            .then(setHabits);
    }, [userId]);

    const addTask = () => {
        if (taskTitle.trim() === '' || taskDate.trim() === '') return;

        const newTask = {
            userId,
            title: taskTitle,
            date: taskDate,
            done: false
        };

        fetch('https://justnotes.xyz/api/tasks', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(newTask)
        })
            .then(res => res.json())
            .then(task => {
                setTasks([...tasks, task]);
                setTaskTitle('');
                setTaskDate('');
            });
    };

    const deleteTask = (id) => {
        fetch(`https://justnotes.xyz/api/tasks/${id}`, {
            method: 'DELETE'
        }).then(() => {
            setTasks(tasks.filter(t => t.id !== id));
        });
    };

    const toggleTaskDone = (id) => {
        const task = tasks.find(t => t.id === id);
        const updated = { ...task, done: !task.done };

        fetch(`https://justnotes.xyz/api/tasks/${id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(updated)
        }).then(() => {
            setTasks(tasks.map(t => t.id === id ? updated : t));
        });
    };

    const addHabit = () => {
        if (habitName.trim() === '') return;

        const newHabit = {
            userId,
            name: habitName,
            frequency: habitFrequency,
            count: 0
        };

        fetch('https://justnotes.xyz/api/habits', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(newHabit)
        })
            .then(res => res.json())
            .then(habit => {
                setHabits([...habits, habit]);
                setHabitName('');
                setHabitFrequency('ежедневно');
            });
    };

    const deleteHabit = (id) => {
        fetch(`https://justnotes.xyz/api/habits/${id}`, {
            method: 'DELETE'
        }).then(() => {
            setHabits(habits.filter(h => h.id !== id));
        });
    };

    const changeHabitCount = (id, delta) => {
        const habit = habits.find(h => h.id === id);
        const updated = { ...habit, count: Math.max(0, habit.count + delta) };

        fetch(`https://justnotes.xyz/api/habits/${id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(updated)
        }).then(() => {
            setHabits(habits.map(h => h.id === id ? updated : h));
        });
    };

    return (
        <div className="container">
            <h1>PiedTracker ALPHA</h1>
            <div className="background"></div>

            <div className="card-item">
                <div className="form-container">
                    <h2>Мои задачи</h2>
                    <div className="form">
                        <input
                            type="text"
                            placeholder="Название задачи"
                            value={taskTitle}
                            className="input-field full-width"
                            onChange={(e) => setTaskTitle(e.target.value)}
                        />
                        <div className="date-button-container">
                            <input
                                type="date"
                                value={taskDate}
                                className="input-field"
                                onChange={(e) => setTaskDate(e.target.value)}
                            />
                            <button onClick={addTask} className="custom-button">
                                Добавить
                            </button>
                        </div>
                    </div>
                </div>

                <div className="card-list">
                    {tasks.map(task => (
                        <div key={task.id} className="card">
                            <div>
                                <p className={`title ${task.done ? 'done' : ''}`}>{task.title}</p>
                                <p className="date">{task.date}</p>
                            </div>
                            <div className="card-actions">
                                <input
                                    type="checkbox"
                                    checked={task.done}
                                    onChange={() => toggleTaskDone(task.id)}
                                />
                                <button onClick={() => deleteTask(task.id)}>✕</button>
                            </div>
                        </div>
                    ))}
                </div>
            </div>

            <div className="card-item">
                <div className="form-container">
                    <h2>Мои привычки</h2>
                    <div className="form">
                        <input
                            type="text"
                            placeholder="Название привычки"
                            value={habitName}
                            className="input-field"
                            onChange={(e) => setHabitName(e.target.value)}
                        />
                        <div className="date-button-container">
                            <select
                                value={habitFrequency}
                                className="custom-select"
                                onChange={(e) => setHabitFrequency(e.target.value)}
                            >
                                <option value="ежедневно">Ежедневно</option>
                                <option value="еженедельно">Еженедельно</option>
                            </select>
                            <button onClick={addHabit} className="custom-button">Добавить</button>
                        </div>
                    </div>
                </div>

                <div className="card-list">
                    {habits.map(habit => (
                        <div key={habit.id} className="card">
                            <div>
                                <p className="title">{habit.name}</p>
                                <p className="date">Частота: {habit.frequency}</p>
                            </div>
                            <div className="card-actions">
                                <button onClick={() => changeHabitCount(habit.id, -1)}>-</button>
                                <span>{habit.count}</span>
                                <button onClick={() => changeHabitCount(habit.id, 1)}>+</button>
                                <button onClick={() => deleteHabit(habit.id)}>✕</button>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
}

export default App;