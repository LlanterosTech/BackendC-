﻿using plantita.ProjectPlantita.plantmanagment.domain.model.aggregates;
using plantita.ProjectPlantita.plantmanagment.domain.model.Entities;
using plantita.ProjectPlantita.plantmanagment.domain.Repositories;
using plantita.ProjectPlantita.plantmanagment.domain.Services;
using plantita.Shared.Domain.Repositories;

namespace plantita.ProjectPlantita.plantmanagment.Application.Internal.CommandServices;

public class MyPlantCommandService : IMyPlantCommandService
{
    private readonly IMyPlantRepository _myPlantRepository;
    private readonly ICareTaskRepository _careTaskRepository;
    private readonly IPlantHealthLogRepository _healthLogRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MyPlantCommandService(
        IMyPlantRepository myPlantRepository,
        ICareTaskRepository careTaskRepository,
        IPlantHealthLogRepository healthLogRepository,
        IUnitOfWork unitOfWork)
    {
        _myPlantRepository = myPlantRepository;
        _careTaskRepository = careTaskRepository;
        _healthLogRepository = healthLogRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<MyPlant> CreateMyPlantAsync(MyPlant myPlant)
    {
        await _myPlantRepository.AddAsync(myPlant);
        await _unitOfWork.CompleteAsync();
        return myPlant;
    }

    public async Task<MyPlant> UpdateMyPlantAsync(Guid myPlantId, MyPlant updatedPlant)
    {
        var existing = await _myPlantRepository.FindByIdGuidAsync(myPlantId);
        if (existing == null) throw new KeyNotFoundException("MyPlant not found");

        existing.CustomName = updatedPlant.CustomName;
        existing.Location = updatedPlant.Location;
        existing.Note = updatedPlant.Note;
        existing.PhotoUrl = updatedPlant.PhotoUrl;
        existing.CurrentStatus = updatedPlant.CurrentStatus;

        await _unitOfWork.CompleteAsync();
        return existing;
    }

    public async Task<bool> DeleteMyPlantAsync(Guid myPlantId)
    {
        var existing = await _myPlantRepository.FindByIdGuidAsync(myPlantId);
        if (existing == null) return false;

        await _myPlantRepository.Delete(existing);
        await _unitOfWork.CompleteAsync();
        return true;
    }

    public async Task<CareTask> MarkCareTaskCompletedAsync(Guid taskId)
    {
        var task = await _careTaskRepository.FindByIdGuidAsync(taskId);
        if (task == null) throw new KeyNotFoundException("Care task not found");

        task.Status = "Completed";
        task.CompletedAt = DateTime.UtcNow;

        await _unitOfWork.CompleteAsync();
        return task;
    }

    public async Task<PlantHealthLog> LogPlantHealthAsync(Guid myPlantId, PlantHealthLog log)
    {
        log.MyPlantId = myPlantId;
        log.Timestamp = DateTime.UtcNow;

        await _healthLogRepository.AddAsync(log);
        await _unitOfWork.CompleteAsync();
        return log;
    }

    public async Task<List<CareTask>> GenerateCareTasksAsync(Guid myPlantId)
    {
        // Aquí podrías basarte en la especie (Plant) y generar el cronograma
        var tasks = new List<CareTask>
        {
            new CareTask
            {
                TaskId = Guid.NewGuid(),
                MyPlantId = myPlantId,
                TaskType = "Riego",
                ScheduledFor = DateTime.UtcNow.AddDays(1),
                Status = "Pending"
            },
            new CareTask
            {
                TaskId = Guid.NewGuid(),
                MyPlantId = myPlantId,
                TaskType = "Fertilizar",
                ScheduledFor = DateTime.UtcNow.AddDays(10),
                Status = "Pending"
            }
        };

        foreach (var task in tasks)
        {
            await _careTaskRepository.AddAsync(task);
        }

        await _unitOfWork.CompleteAsync();
        return tasks;
    }
}