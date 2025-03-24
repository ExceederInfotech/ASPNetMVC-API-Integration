using EventManagement.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace EventManagementApp.Controllers
{
    public class EventsController : Controller
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        public EventsController(IConfiguration configuration)
        {
            _config = configuration;
            _client = new HttpClient();
            string baseAdd = _config.GetValue<string>("Settings:baseAddress");
            Uri baseAddress = new Uri(baseAdd);
            _client.BaseAddress = baseAddress;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<EventDetailsDTO> EventList = new List<EventDetailsDTO>();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "EventSchedule/GetAllEventDetails").Result;
            if (response.IsSuccessStatusCode)
            {
                string Data = response.Content.ReadAsStringAsync().Result;
                EventList = JsonConvert.DeserializeObject<List<EventDetailsDTO>>(Data);
            }
            return View(EventList);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEventDTO EventDTO)
        {
            try
            {
                string data = JsonConvert.SerializeObject(EventDTO);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync(_client.BaseAddress + "EventSchedule/SaveEvent", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Data save successfully!";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                throw;
            }
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EventDetails(int id)
        {
            List<EventDetailsDTO> EventList = new List<EventDetailsDTO>();
            EventDetailsDTO eventDetailsDTO = new EventDetailsDTO();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "EventSchedule/GetEventDetailsByEventID/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                //EventList = JsonConvert.DeserializeObject<List<EventDetailsDTO>>(data);
                eventDetailsDTO = JsonConvert.DeserializeObject<EventDetailsDTO>(data);
            }

            List<EventScheduleDTO> eventSchedule = new List<EventScheduleDTO>();
            CreateEventDTO createEvent = new CreateEventDTO();
            HttpResponseMessage responsen = _client.GetAsync(_client.BaseAddress + "EventSchedule/GetScheduleDetailsByEventID/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = responsen.Content.ReadAsStringAsync().Result;
                eventSchedule = JsonConvert.DeserializeObject<List<EventScheduleDTO>>(data);
            }
            eventDetailsDTO.EventSchedule = eventSchedule.ToList();
            return View(eventDetailsDTO);
        }

        public async Task<IActionResult> DeleteEvent(int id)
        {
            try
            {
                HttpResponseMessage response = _client.DeleteAsync(_client.BaseAddress + "EventSchedule/DeleteEventByEventID/"
                                                + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IActionResult Errorpage()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DisplayEventDetails(int id)
        {
            List<EventScheduleDTO> EventList = new List<EventScheduleDTO>();
            CreateEventDTO createEvent = new CreateEventDTO();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "EventSchedule/GetScheduleDetailsByEventID/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                EventList = JsonConvert.DeserializeObject<List<EventScheduleDTO>>(data);
            }
            return PartialView("DisplayEventDetails", EventList);
        }
    }
}
