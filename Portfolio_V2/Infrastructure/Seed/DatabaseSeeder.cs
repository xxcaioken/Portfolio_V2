using Portfolio_V2.Application.Services;
using Portfolio_V2.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Portfolio_V2.Infrastructure.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            if (!await db.Users.AnyAsync())
            {
                (string hash, string salt) = AuthService.CreatePasswordHash("admin123");
                List<User> users =
                [
                    new User { Id = Guid.NewGuid(), Username = "admin", PasswordHash = hash, PasswordSalt = salt, Role = Role.Admin },
                    new User { Id = Guid.NewGuid(), Username = "user", PasswordHash = hash, PasswordSalt = salt, Role = Role.User }
                ];
                db.Users.AddRange(users);
                await db.SaveChangesAsync();
            }

            if (!await db.AboutInfos.AnyAsync())
            {
                var about = new AboutInfo
                {
                    Id = Guid.NewGuid(),
                    Name = "Caio Wendhausem Kormives",
                    Title = "Desenvolvedor Full Stack",
                    Summary = "Desenvolvedor Full Stack com 4 anos de experiência na construção de sistemas escaláveis e resolução de problemas críticos em produção. Possuo sólida proficiência em ecossistemas robustos como Java e .NET (C#), aliados ao domínio de React.js e Angular. Desafiei-me recentemente em arquiteturas modernas utilizando Elixir (Phoenix), GraphQL e programação funcional. Experiente na estabilização de ambientes multi-tenancy, migração de sistemas legados e implementação de cultura de testes E2E com Cypress.",
                    Location = "Florianópolis, SC",
                    Phone = "+5548998459412",
                    Email = "cwkormives@gmail.com",
                    Linkedin = "https://www.linkedin.com/in/caio-kormives/",
                    Github = "https://github.com/xxcaioken",
                    FooterNote = "Desenvolvido com React & ASP.NET Core",
                    Socials =
                    [
                        new SocialLink { Id = Guid.NewGuid(), Label = "LinkedIn", Url = "https://www.linkedin.com/in/caio-kormives/", IconKey = "linkedin" },
                        new SocialLink { Id = Guid.NewGuid(), Label = "GitHub", Url = "https://github.com/xxcaioken", IconKey = "github" },
                        new SocialLink { Id = Guid.NewGuid(), Label = "Email", Url = "mailto:cwkormives@gmail.com", IconKey = "email" },
                        new SocialLink { Id = Guid.NewGuid(), Label = "WhatsApp", Url = "https://wa.me/5548998459412", IconKey = "whatsapp" },
                    ]
                };
                db.AboutInfos.Add(about);
                await db.SaveChangesAsync();
            }

            if (!await db.Experiences.AnyAsync())
            {
                var experiences = new List<ExperienceItem>
                {
                    new ExperienceItem
                    {
                        Id = Guid.NewGuid(),
                        Company = "C.M.",
                        Role = "Desenvolvedor Full Stack",
                        StartDate = new DateOnly(2025, 7, 1),
                        EndDate = new DateOnly(2025, 12, 1),
                        Bullets =
                        [
                            "Desenvolvimento de aplicações de alta performance com Elixir (Phoenix) e GraphQL (Absinthe)",
                            "Implementação de testes E2E com Cypress e desenvolvimento de plugin para Adobe Illustrator",
                            "Gestão de dados com PostgreSQL e Min.io em ambiente Linux (Manjaro)"
                        ]
                    },
                    new ExperienceItem
                    {
                        Id = Guid.NewGuid(),
                        Company = "Portal Postal",
                        Role = "Desenvolvedor Full Stack",
                        StartDate = new DateOnly(2024, 12, 1),
                        EndDate = new DateOnly(2025, 6, 1),
                        Bullets =
                        [
                            "Atuação com Java e Angular em sistemas corporativos e PHP (WooCommerce) para e-commerce",
                            "Liderança técnica na migração crítica BLING (v2 para v3) e integração de APIs de logística e pagamentos",
                            "Otimização de performance sistêmica baseada em análise de métricas e dados"
                        ]
                    },
                    new ExperienceItem
                    {
                        Id = Guid.NewGuid(),
                        Company = "Tech6 Group - FATHOM",
                        Role = "Desenvolvedor Full Stack",
                        StartDate = new DateOnly(2022, 4, 1),
                        EndDate = new DateOnly(2024, 7, 1),
                        Bullets =
                        [
                            "Desenvolvimento .NET (C#) e React para sistemas multi-tenancy de alta escala",
                            "Reestruturação da camada de repositórios para desacoplamento e evolução de sistemas em produção (PRD)",
                            "Estabilização de ambientes críticos e otimização de migrations de banco de dados sob metodologias ágeis"
                        ]
                    }
                };
                db.Experiences.AddRange(experiences);
                await db.SaveChangesAsync();
            }

            if (!await db.Habilities.AnyAsync())
            {
                var habilities = new List<HabilityItem>
                {
                    new HabilityItem
                    {
                        Id = Guid.NewGuid(),
                        Hability = "Backend",
                        Bullets =
                        [
                            new HabilityBullet { Id = Guid.NewGuid(), Text = "C# / ASP.NET Core", Badge = "dotnet" },
                            new HabilityBullet { Id = Guid.NewGuid(), Text = "Java / Spring Boot", Badge = "java" },
                            new HabilityBullet { Id = Guid.NewGuid(), Text = "Elixir / Phoenix", Badge = "elixir" },
                            new HabilityBullet { Id = Guid.NewGuid(), Text = "Node.js", Badge = "nodejs" },
                            new HabilityBullet { Id = Guid.NewGuid(), Text = "PHP", Badge = "php" },
                            new HabilityBullet { Id = Guid.NewGuid(), Text = "Python", Badge = "python" },
                        ]
                    },
                    new HabilityItem
                    {
                        Id = Guid.NewGuid(),
                        Hability = "Frontend",
                        Bullets =
                        [
                            new HabilityBullet { Id = Guid.NewGuid(), Text = "React.js", Badge = "react" },
                            new HabilityBullet { Id = Guid.NewGuid(), Text = "Angular", Badge = "angular" },
                            new HabilityBullet { Id = Guid.NewGuid(), Text = "TypeScript", Badge = "typescript" },
                            new HabilityBullet { Id = Guid.NewGuid(), Text = "JavaScript (ES6+)", Badge = "javascript" },
                            new HabilityBullet { Id = Guid.NewGuid(), Text = "GraphQL", Badge = "graphql" },
                        ]
                    },
                    new HabilityItem
                    {
                        Id = Guid.NewGuid(),
                        Hability = "DevOps & Cloud",
                        Bullets =
                        [
                            new HabilityBullet { Id = Guid.NewGuid(), Text = "Docker", Badge = "docker" },
                            new HabilityBullet { Id = Guid.NewGuid(), Text = "Git / CI/CD", Badge = "git" },
                            new HabilityBullet { Id = Guid.NewGuid(), Text = "Azure", Badge = "azure" },
                            new HabilityBullet { Id = Guid.NewGuid(), Text = "Amazon S3", Badge = "aws" },
                            new HabilityBullet { Id = Guid.NewGuid(), Text = "Linux", Badge = "linux" },
                        ]
                    },
                    new HabilityItem
                    {
                        Id = Guid.NewGuid(),
                        Hability = "Banco de Dados",
                        Bullets =
                        [
                            new HabilityBullet { Id = Guid.NewGuid(), Text = "PostgreSQL", Badge = "postgresql" },
                            new HabilityBullet { Id = Guid.NewGuid(), Text = "SQL Server", Badge = "sqlserver" },
                        ]
                    },
                    new HabilityItem
                    {
                        Id = Guid.NewGuid(),
                        Hability = "Testes",
                        Bullets =
                        [
                            new HabilityBullet { Id = Guid.NewGuid(), Text = "Cypress (E2E)", Badge = "cypress" },
                        ]
                    }
                };
                db.Habilities.AddRange(habilities);
                await db.SaveChangesAsync();
            }

            if (!await db.AditionalInfos.AnyAsync())
            {
                var educations = new List<AditionalInfoItem>
                {
                    new AditionalInfoItem
                    {
                        Id = Guid.NewGuid(),
                        AditionalInfo = "Formação Acadêmica",
                        Bullets =
                        [
                            new AditionalInfoBullet
                            {
                                Id = Guid.NewGuid(),
                                Text = "Técnico em Desenvolvimento de Software",
                                StartDate = new DateOnly(2019, 1, 1),
                                EndDate = new DateOnly(2022, 12, 1),
                                Level = "Concluído"
                            },
                            new AditionalInfoBullet
                            {
                                Id = Guid.NewGuid(),
                                Text = "Análise e Desenvolvimento de Sistemas (ADS) — Florianópolis",
                                StartDate = new DateOnly(2023, 1, 1),
                                EndDate = null,
                                Level = "Cursando"
                            }
                        ]
                    },
                    new AditionalInfoItem
                    {
                        Id = Guid.NewGuid(),
                        AditionalInfo = "Idiomas",
                        Bullets =
                        [
                            new AditionalInfoBullet { Id = Guid.NewGuid(), Text = "Português", Level = "Nativo" },
                            new AditionalInfoBullet { Id = Guid.NewGuid(), Text = "Inglês", Level = "Intermediário" }
                        ]
                    }
                };
                db.AditionalInfos.AddRange(educations);
                await db.SaveChangesAsync();
            }

            if (!await db.Set<KeyTaskBullet>().AnyAsync())
            {
                var keyTasks = new List<KeyTaskBullet>
                {
                    new KeyTaskBullet
                    {
                        Id = Guid.NewGuid(),
                        KeyTask = "Migração BLING v2 → v3",
                        description = "Liderança técnica na migração crítica da API BLING de v2 para v3 no Portal Postal, garantindo continuidade operacional do e-commerce e integrações com APIs de logística e pagamentos.",
                        Technologies =
                        [
                            new KeyTaskTechnology { Id = Guid.NewGuid(), Technology = "Java", TechnologyBadge = "java" },
                            new KeyTaskTechnology { Id = Guid.NewGuid(), Technology = "Angular", TechnologyBadge = "angular" },
                            new KeyTaskTechnology { Id = Guid.NewGuid(), Technology = "PHP", TechnologyBadge = "php" },
                        ]
                    },
                    new KeyTaskBullet
                    {
                        Id = Guid.NewGuid(),
                        KeyTask = "Sistema Multi-Tenancy FATHOM",
                        description = "Desenvolvimento e estabilização de sistema multi-tenancy de alta escala com .NET (C#) e React. Reestruturação completa da camada de repositórios para desacoplamento e otimização de migrations em produção.",
                        Technologies =
                        [
                            new KeyTaskTechnology { Id = Guid.NewGuid(), Technology = "C# / .NET", TechnologyBadge = "dotnet" },
                            new KeyTaskTechnology { Id = Guid.NewGuid(), Technology = "React", TechnologyBadge = "react" },
                            new KeyTaskTechnology { Id = Guid.NewGuid(), Technology = "SQL Server", TechnologyBadge = "sqlserver" },
                        ]
                    },
                    new KeyTaskBullet
                    {
                        Id = Guid.NewGuid(),
                        KeyTask = "Plugin Adobe Illustrator",
                        description = "Desenvolvimento de plugin para Adobe Illustrator integrado a pipeline de dados com Elixir, GraphQL e armazenamento Min.io, com cobertura de testes E2E via Cypress.",
                        Technologies =
                        [
                            new KeyTaskTechnology { Id = Guid.NewGuid(), Technology = "Elixir", TechnologyBadge = "elixir" },
                            new KeyTaskTechnology { Id = Guid.NewGuid(), Technology = "GraphQL", TechnologyBadge = "graphql" },
                            new KeyTaskTechnology { Id = Guid.NewGuid(), Technology = "Cypress", TechnologyBadge = "cypress" },
                        ]
                    },
                    new KeyTaskBullet
                    {
                        Id = Guid.NewGuid(),
                        KeyTask = "Portfolio V2",
                        description = "Desenvolvimento do portfólio pessoal com backend em ASP.NET Core 9, banco de dados PostgreSQL (Neon), frontend em React 19 + TypeScript + Vite, com deploy automatizado via GitHub Actions para Azure.",
                        Technologies =
                        [
                            new KeyTaskTechnology { Id = Guid.NewGuid(), Technology = "ASP.NET Core", TechnologyBadge = "dotnet" },
                            new KeyTaskTechnology { Id = Guid.NewGuid(), Technology = "React", TechnologyBadge = "react" },
                            new KeyTaskTechnology { Id = Guid.NewGuid(), Technology = "PostgreSQL", TechnologyBadge = "postgresql" },
                            new KeyTaskTechnology { Id = Guid.NewGuid(), Technology = "Azure", TechnologyBadge = "azure" },
                        ]
                    }
                };
                db.Set<KeyTaskBullet>().AddRange(keyTasks);
                await db.SaveChangesAsync();
            }
        }
    }
}
