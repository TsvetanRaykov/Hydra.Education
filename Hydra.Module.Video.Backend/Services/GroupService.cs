﻿using System;
using System.Collections.Generic;

namespace Hydra.Module.Video.Backend.Services
{
    using System.Linq;
    using Models;
    using Microsoft.EntityFrameworkCore;
    using Contracts;
    using Data;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    public class GroupService : ServiceBase, IGroupService
    {
        private readonly VideoDbContext _dbContext;

        public GroupService(ILogger<GroupService> logger, VideoDbContext dbContext)
            : base(logger)
        {
            _dbContext = dbContext;
        }

        public async Task<string> CreateGroupAsync(string name, string description, string imageUrl, int classId)
        {
            var newGroup = new VideoGroup()
            {
                Name = name,
                Description = description,
                ImageUrl = imageUrl,
                VideoClassId = classId
            };

            await _dbContext.VideoGroups.AddAsync(newGroup);

            return await UpdateDbAsync(_dbContext);
        }

        public async Task<GroupResponseDto> GetGroupAsync(int id)
        {
            var group = await _dbContext
                      .VideoGroups
                      .Include(g => g.VideoClass)
                      .Include(g => g.Users)
                      .Include(g => g.Playlists)
                      .ThenInclude(p => p.Playlist.Videos)
                      .ThenInclude(v => v.Video)
                      .Include(g => g.Playlists)
                      .ThenInclude(p => p.Playlist)
                      .ThenInclude(p => p.VideoGroups)
                      .ThenInclude(g => g.Group)
                      .ThenInclude(g => g.VideoClass)
                      .FirstOrDefaultAsync(c => c.Id.Equals(id));

            if (group == null)
            {
                return null;
            }

            return new GroupResponseDto()
            {
                Name = group.Name,
                ImageUrl = group.ImageUrl,
                Description = group.Description,
                Id = group.Id,
                Playlists = group.Playlists.Select(p => new PlaylistResponseDto
                {
                    Id = p.Playlist.Id,
                    Name = p.Playlist.Name,
                    Description = p.Playlist.Description,
                    ImageUrl = p.Playlist.ImageUrl,
                    VideoGroups = p.Playlist.VideoGroups.Select(g => new GroupResponseDto
                    {
                        Id = g.Group.Id,
                        Name = g.Group.Name,
                        Description = g.Group.Description,
                        ImageUrl = g.Group.ImageUrl,
                        Class = new ClassResponseDto
                        {
                            Id = g.Group.VideoClass.Id,
                            Name = g.Group.VideoClass.Name,
                            Description = g.Group.VideoClass.Description,
                            ImageUrl = g.Group.VideoClass.ImageUrl
                        }
                    }).ToList(),
                    Videos = p.Playlist.Videos.Select(v => new VideoResponseDto
                    {
                        Name = v.Video.Name,
                        Description = v.Video.Description,
                        Id = v.VideoId,
                        UploadedBy = v.Video.UploadedBy,
                        Url = v.Video.Url
                    }).ToList(),
                }).ToList(),
                Users = group.Users.Select(u => u.UserId).ToList(),
                Class = new ClassResponseDto()
                {
                    Name = group.VideoClass.Name,
                    Id = group.VideoClass.Id,
                    ImageUrl = group.VideoClass.ImageUrl,
                    TrainerId = group.VideoClass.TrainerId,
                    Description = group.VideoClass.Description
                }
            };

        }

        public async Task<string> UpdateGroupAsync(int id, string name, string description, string imageUrl)
        {
            var @group = await _dbContext.FindAsync<VideoGroup>(id);

            if (@group == null) return "Group not found.";

            var toUpdate = false;
            if (@group.Name != name)
            {
                @group.Name = name;
                toUpdate = true;
            }
            if (@group.Description != description)
            {
                @group.Description = description;
                toUpdate = true;
            }
            if (@group.ImageUrl != imageUrl)
            {
                @group.ImageUrl = imageUrl;
                toUpdate = true;
            }

            if (toUpdate)
            {
                return await UpdateDbAsync(_dbContext);
            }

            return null;
        }

        public async Task<string> SetUsersAsync(int groupId, IEnumerable<string> usersIds)
        {
            var @group = await _dbContext.VideoGroups
                .Include(g => g.Users)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            if (@group == null) return "Group not found";

            var users = usersIds.Select(id => new UserToGroup { UserId = id, Group = @group });

            @group.Users.Clear();

            foreach (var userToGroup in users)
            {
                @group.Users.Add(userToGroup);
            }

            return await UpdateDbAsync(_dbContext);

        }

        public async Task<string> AddPlaylist(int groupId, int playlistId)
        {
            var group = await _dbContext.VideoGroups
                .Include(g => g.Playlists)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null) return "Group not found.";

            var playlist = await _dbContext.Playlists.FindAsync(playlistId);

            if (playlist == null) return "Playlist not found.";

            group.Playlists.Add(new PlaylistToGroup
            {
                Playlist = playlist
            });

            return await UpdateDbAsync(_dbContext);
        }

        public async Task<string> RemovePlaylist(int groupId, int playlistId)
        {
            var group = await _dbContext.VideoGroups
                .Include(g => g.Playlists)
                .FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null) return "Group not found.";

            var playlist = group.Playlists.FirstOrDefault(p => p.PlaylistId == playlistId);

            if (playlist != null)
            {
                group.Playlists.Remove(playlist);
            }

            return await UpdateDbAsync(_dbContext);
        }

        public async Task<string> DeleteGroupAsync(int id)
        {
            var group = await _dbContext.VideoGroups
                .Include(g => g.Playlists)
                .Include(g => g.Users)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null) return "Group not found.";

            _dbContext.Remove(group);

            return await UpdateDbAsync(_dbContext);
        }
    }
}