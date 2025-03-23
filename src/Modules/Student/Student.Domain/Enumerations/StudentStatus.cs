using Ardalis.SmartEnum;
using Newtonsoft.Json;

namespace Student.Domain.Enumerations
{
    [method: JsonConstructor]
    public class StudentStatus(string name, int value) : SmartEnum<StudentStatus>(name, value)
    {
        public static readonly StudentStatus Default = new DefaultStatus();
        public static readonly StudentStatus Active = new ActiveStatus();
        public static readonly StudentStatus Blocked = new BlockedStatus();
        public static readonly StudentStatus PendingProfile = new PendingProfileStatus();

        public static implicit operator StudentStatus(string name)
            => FromName(name);

        public static implicit operator StudentStatus(int value)
            => FromValue(value);

        public static implicit operator string(StudentStatus status)
            => status.Name;

        public static implicit operator int(StudentStatus status)
            => status.Value;

        public class DefaultStatus() : StudentStatus(nameof(Default), 0) { }
        public class ActiveStatus() : StudentStatus(nameof(Active), 1) { }
        public class PendingProfileStatus() : StudentStatus(nameof(PendingProfile), 2) { }
        public class BlockedStatus() : StudentStatus(nameof(Blocked), 3) { }
    }
}
