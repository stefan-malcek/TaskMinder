﻿using System.Reflection;
using System.Runtime.Serialization;
using AutoMapper;
using TaskFree.Application.Common.Interfaces;
using TaskFree.Application.Common.Models;
using TaskFree.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using TaskFree.Application.TodoLists.Queries.GetTodos;
using TaskFree.Domain.Entities;
using NUnit.Framework;

namespace TaskFree.Application.UnitTests.Common.Mappings;

public class MappingTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingTests()
    {
        _configuration = new MapperConfiguration(config => 
            config.AddMaps(Assembly.GetAssembly(typeof(IApplicationDbContext))));

        _mapper = _configuration.CreateMapper();
    }

    [Test]
    public void ShouldHaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Test]
    [TestCase(typeof(NoteList), typeof(TodoListDto))]
    [TestCase(typeof(TodoItem), typeof(TodoItemDto))]
    [TestCase(typeof(NoteList), typeof(LookupDto))]
    [TestCase(typeof(TodoItem), typeof(LookupDto))]
    [TestCase(typeof(TodoItem), typeof(TodoItemBriefDto))]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);

        _mapper.Map(instance, source, destination);
    }

    private object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;

        // Type without parameterless constructor
        // TODO: Figure out an alternative approach to the now obsolete `FormatterServices.GetUninitializedObject` method.
#pragma warning disable SYSLIB0050 // Type or member is obsolete
        return FormatterServices.GetUninitializedObject(type);
#pragma warning restore SYSLIB0050 // Type or member is obsolete
    }
}
