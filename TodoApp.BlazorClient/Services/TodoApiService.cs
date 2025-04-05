using System.Net.Http.Json;
using TodoApp.Domain.Entities;

namespace TodoApp.BlazorClient.Services;

public class TodoApiService
{
    private readonly HttpClient _httpClient;

    public TodoApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Obtiene la lista de TodoItems (se asume que la Web API tiene un endpoint GET /api/todolist/items)
    public async Task<List<TodoItem>> GetTodoItemsAsync()
    {
        var domainItems = await _httpClient.GetFromJsonAsync<List<TodoItem>>("api/todolist/items");
        return domainItems?.Select(MapToClientModel).ToList() ?? new List<TodoItem>();
    }

    public async Task<int> AddTodoItemAsync(string title, 
                                            string description, 
                                            string category)
    {
        // Se utiliza el endpoint con query string para agregar el item
        var response = await _httpClient.PostAsync(
            $"api/todolist/add?title={Uri.EscapeDataString(title)}&description=" +
            $"{Uri.EscapeDataString(description)}&category={Uri.EscapeDataString(category)}", 
            null);

        response.EnsureSuccessStatusCode();
        // Verificamos que la respuesta sea exitosa
        // Se espera que la respuesta sea JSON con el ID creado
        var result = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        if (result != null && result.ContainsKey("Id"))
        {
            return System.Convert.ToInt32(result["Id"]);
        }
        return 0;
    }

    public async Task RegisterProgressionAsync(int id, 
                                               DateTime dateTime, 
                                               decimal percent)
    {
        // Usamos formato ISO 8601 para la fecha (dateTime:o)
        var url = $"api/todolist/progression?id={id}&dateTime={dateTime:o}&percent={percent}";
        var response = await _httpClient.PostAsync(url, null);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateTodoItemAsync(int id, 
                                          string newDescription)
    {
        var response = await _httpClient.PutAsync(
            $"api/todolist/update?id={id}&newDescription={Uri.EscapeDataString(newDescription)}", 
            null);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveTodoItemAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/todolist/remove?id={id}");
        response.EnsureSuccessStatusCode();
    }


    private TodoItem MapToClientModel(TodoApp.Domain.Entities.TodoItem domainItem) {
        return new TodoItem(
            domainItem.Id,
            domainItem.Title,
            domainItem.Description,
            domainItem.Category
        );
    }
    
}