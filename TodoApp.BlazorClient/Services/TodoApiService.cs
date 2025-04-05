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
    //public async Task<List<TodoItem>> GetTodoItemsAsync()
    public async Task<List<TodoApp.BlazorClient.Models.TodoItem>> GetTodoItemsAsync()
    {
        var domainItems = await _httpClient.GetFromJsonAsync<List<TodoItem>>("api/todolist/items");
        return domainItems?.Select(MapToClientModel).ToList();
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

        string encodedDateTime = Uri.EscapeDataString(dateTime.ToString("o"));
         var url = $"api/todolist/progression?id={id}&dateTime={encodedDateTime}&percent={percent}";

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


    //private TodoItem MapToClientModel(TodoApp.Domain.Entities.TodoItem domainItem) {
    //    return new TodoItem(
    //        domainItem.Id,
    //        domainItem.Title,
    //        domainItem.Description,
    //        domainItem.Category
    //    );
    //}
    private TodoApp.BlazorClient.Models.TodoItem MapToClientModel(TodoApp.Domain.Entities.TodoItem domainItem)
    {
        return new TodoApp.BlazorClient.Models.TodoItem
        {
            Id = domainItem.Id,
            Title = domainItem.Title,
            Description = domainItem.Description,
            Category = domainItem.Category,
            IsCompleted = domainItem.IsCompleted,
            Progressions = domainItem.Progressions?
            .Select(p => new TodoApp.BlazorClient.Models.Progression
            {
                Date = p.Date, 
                Percent = p.Percent
            })
            .ToList() ?? new List<TodoApp.BlazorClient.Models.Progression>()
        };
    }
   
}