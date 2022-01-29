import { useQuery } from 'react-query';
import Box from "@mui/material/Box";
import CircularProgress from '@mui/material/CircularProgress';
import { useState } from 'react';

const requestHeaders: HeadersInit = new Headers();
requestHeaders.set('Content-Type', 'application/json');
requestHeaders.set('X-CSRF', '1');

async function fetchTodos() {
  const response = await fetch("todos", {
    headers: requestHeaders,
  });

  if (response.ok) {
    return await response.json();
  }

  return false;
}

async function createTodo(e: { preventDefault: () => void; }, todoName:string, todoDate: string, refetch: any) {
  e.preventDefault();

  if (!todoName || !todoDate) {
    alert('TodoName or todoDate is empty');
    return;
  }

  const response = await fetch("todos", {
    method: "POST",
    headers: requestHeaders,
    body: JSON.stringify({
      name: todoName,
      date: todoDate,
    }),
  });

  if (response.ok) {
    refetch();
  }
}

async function deleteTodo(id: any, refetch: any) {
  const response = await fetch(`todos/${id}`, {
      method: "DELETE",
      headers: requestHeaders
  });

  if (response.ok) {
    refetch();
  }
}

export default function Home() {
  const { data: todos, isLoading, error, refetch } = useQuery<any, ErrorConstructor>('todos', fetchTodos);
  const [todoName, setTodoName] = useState("Do Task");
  const [todoDate, setTodoDate] = useState("2021-03-01");
  
  return (
    <>
      <div className="row">
          <div className="col">
            <h3>Add New</h3>
          </div>
          <div className="form-inline">
            <label htmlFor="date">Todo Date</label>
            <input
              className="form-control"
              type="date"
              value={todoDate}
              onChange={(e) => setTodoDate(e.target.value)}
            />
            <label htmlFor="name">Todo Name</label>
            <input
              className="form-control"
              value={todoName}
              onChange={(e) => setTodoName(e.target.value)}
            />
            <button
              className="form-control btn-success"
              onClick={async (e) => createTodo(e, todoName, todoDate, refetch)}
            >
              Create
            </button>
          </div>
      </div>
      {isLoading && (
          <div>
              <Box sx={{ display: 'flex' }}>
                <CircularProgress color="secondary" />
              </Box>
          </div>
      )}
      {error && (
        <div>Something went wrong while fetching todos.</div>
      )}
      {todos && !isLoading && !error && (
        <div>
          <div className="banner">
              <h1>TODOs</h1>
          </div>
          <div className="row">
            <table className="table table-striped table-sm">
              <thead>
                <tr>
                  <th/>
                  <th>Id</th>
                  <th>Date</th>
                  <th>Note</th>
                  <th>User</th>
                </tr>
              </thead>
              <tbody>
                {todos.map((todo: any) => (
                  <tr key={todo.id}>
                    <td>
                      <button
                        onClick={async () => deleteTodo(todo.id, refetch)}
                        className="btn btn-danger"
                      >
                        delete
                      </button>
                    </td>
                    <td>{todo.id}</td>
                    <td>{todo.date}</td>
                    <td>{todo.name}</td>
                    <td>{todo.user}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}
    </>
  );
}

/*class Home2{
  static displayName = Home.name;

  constructor(props: any) {
    super(props);
    this.state = {
      todos: [],
      loading: true,
      error: null,
      todoName: "Do something",
      todoDate: "2021-03-01",
    };

    this.createTodo = this.createTodo.bind(this);
    this.deleteTodo = this.deleteTodo.bind(this);
  }

  componentDidMount() {
    (async () => this.populateTodos())();
  }

  async populateTodos() {
    const response = await fetch("todos", {
      headers: requestHeaders
    });
    if (response.ok) {
      const data = await response.json();
      this.setState({ todos: data, loading: false, error: null });
    } else if (response.status !== 401) {
      this.setState({ error: response.status });
    }
  }

  async createTodo(e: { preventDefault: () => void; }) {
    e.preventDefault();
    const response = await fetch("todos", {
      method: "POST",
      headers: requestHeaders,
      body: JSON.stringify({
        name: this.state.todoName,
        date: this.state.todoDate,
      }),
    });

    if (response.ok) {
      var item = await response.json();
      this.setState({
        todos: [...this.state.todos, item],
        todoName: "Do something",
        todoDate: "2021-03-02",
      });
    } else {
      this.setState({ error: response.status });
    }
  }

  async deleteTodo(id: any) {
    
    const response = await fetch(`todos/${id}`, {
        method: "DELETE",
        headers: requestHeaders
      });
    if (response.ok) {
      const todos = this.state.todos.filter((x) => x.id !== id);
      this.setState({ todos });
    } else {
      this.setState({ error: response.status });
    }
  }

  render() {
    return (
      <>
        <div className="banner">
          <h1>TODOs</h1>
        </div>

        <div className="row">
          <div className="col">
            <h3>Add New</h3>
          </div>
          <div className="form-inline">
            <label htmlFor="date">Todo Date</label>
            <input
              className="form-control"
              type="date"
              value={this.state.todoDate}
              onChange={(e) => this.setState({ todoDate: e.target.value })}
            />
            <label htmlFor="name">Todo Name</label>
            <input
              className="form-control"
              value={this.state.todoName}
              onChange={(e) => this.setState({ todoName: e.target.value })}
            />
            <button
              className="form-control btn-success"
              onClick={this.createTodo}
            >
              Create
            </button>
          </div>
        </div>
        {this.state.error !== null && (
          <div className="row">
            <div className="col">
              <div className="alert alert-warning hide">
                <strong>Error: </strong>
                <span>{this.state.error}</span>
              </div>
            </div>
          </div>
        )}

        <div className="row">
          <table className="table table-striped table-sm">
            <thead>
              <tr>
                <th/>
                <th>Id</th>
                <th>Date</th>
                <th>Note</th>
                <th>User</th>
              </tr>
            </thead>
            <tbody>
              {this.state.todos.map((todo) => (
                <tr key={todo.id}>
                  <td>
                    <button
                      onClick={async () => this.deleteTodo(todo.id)}
                      className="btn btn-danger"
                    >
                      delete
                    </button>
                  </td>
                  <td>{todo.id}</td>
                  <td>{todo.date}</td>
                  <td>{todo.name}</td>
                  <td>{todo.user}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </>
    );
  }
}*/