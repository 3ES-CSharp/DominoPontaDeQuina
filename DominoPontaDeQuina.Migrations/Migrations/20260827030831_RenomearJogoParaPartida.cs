using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DominoPontaDeQuina.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class RenomearJogoParaPartida : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParticipacaoJogo_Jogadores_JogadorId",
                table: "ParticipacaoJogo");

            migrationBuilder.DropForeignKey(
                name: "FK_ParticipacaoJogo_Jogos_JogoId",
                table: "ParticipacaoJogo");

            migrationBuilder.RenameTable(
                name: "Jogos",
                newName: "Partidas");

            migrationBuilder.RenameTable(
                name: "ParticipacaoJogo",
                newName: "ParticipacoesPartida");

            migrationBuilder.RenameColumn(
                name: "JogoId",
                table: "ParticipacoesPartida",
                newName: "PartidaId");

            migrationBuilder.RenameIndex(
                name: "IX_ParticipacaoJogo_JogadorId",
                table: "ParticipacoesPartida",
                newName: "IX_ParticipacoesPartida_JogadorId");

            migrationBuilder.RenameIndex(
                name: "IX_ParticipacaoJogo_JogoId",
                table: "ParticipacoesPartida",
                newName: "IX_ParticipacoesPartida_PartidaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParticipacoesPartida_Jogadores_JogadorId",
                table: "ParticipacoesPartida",
                column: "JogadorId",
                principalTable: "Jogadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParticipacoesPartida_Partidas_PartidaId",
                table: "ParticipacoesPartida",
                column: "PartidaId",
                principalTable: "Partidas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParticipacoesPartida_Jogadores_JogadorId",
                table: "ParticipacoesPartida");

            migrationBuilder.DropForeignKey(
                name: "FK_ParticipacoesPartida_Partidas_PartidaId",
                table: "ParticipacoesPartida");

            migrationBuilder.RenameIndex(
                name: "IX_ParticipacoesPartida_JogadorId",
                table: "ParticipacoesPartida",
                newName: "IX_ParticipacaoJogo_JogadorId");

            migrationBuilder.RenameIndex(
                name: "IX_ParticipacoesPartida_PartidaId",
                table: "ParticipacoesPartida",
                newName: "IX_ParticipacaoJogo_JogoId");

            migrationBuilder.RenameColumn(
                name: "PartidaId",
                table: "ParticipacoesPartida",
                newName: "JogoId");

            migrationBuilder.RenameTable(
                name: "ParticipacoesPartida",
                newName: "ParticipacaoJogo");

            migrationBuilder.RenameTable(
                name: "Partidas",
                newName: "Jogos");

            migrationBuilder.AddForeignKey(
                name: "FK_ParticipacaoJogo_Jogadores_JogadorId",
                table: "ParticipacaoJogo",
                column: "JogadorId",
                principalTable: "Jogadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParticipacaoJogo_Jogos_JogoId",
                table: "ParticipacaoJogo",
                column: "JogoId",
                principalTable: "Jogos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
